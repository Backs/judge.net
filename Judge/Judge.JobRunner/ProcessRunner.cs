using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Judge.Runner.Abstractions;
using static Judge.JobRunner.Pinvoke;

namespace Judge.JobRunner;

public static class ProcessRunner
{
    public static RunResult RunProcessWithLimits(RunOptions options)
    {
        var job = CreateJobObject(IntPtr.Zero, null);
        if (job == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        try
        {
            SetExtendedInfo(job, options.TimeLimit, options.MemoryLimitBytes);
            SetUiRestrictions(job);

            var ioCompletionPort = CreateIoCompletionPort(-1, IntPtr.Zero, 1U, 1U);
            if (ioCompletionPort == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            try
            {
                AssociateCompletionPortInformation(job, ioCompletionPort);
                var securityAttributes = new SecurityAttributes
                {
                    nLength = Marshal.SizeOf<SecurityAttributes>(),
                    bInheritHandle = 1
                };

                var input = CreateFile(
                    Path.Combine(options.WorkingDirectory, options.Input),
                    DesiredAccess.GENERIC_READ, 0, ref securityAttributes,
                    CreationDisposition.OPEN_EXISTING, 0, IntPtr.Zero);

                var output = CreateFile(
                    Path.Combine(options.WorkingDirectory, options.Output),
                    DesiredAccess.GENERIC_WRITE, 0, ref securityAttributes,
                    CreationDisposition.CREATE_ALWAYS, 0, IntPtr.Zero);

                var startupInfo = new StartupInfo();
                startupInfo.cb = Marshal.SizeOf(startupInfo);
                startupInfo.dwFlags = 0x00000100;
                startupInfo.hStdInput = input;
                startupInfo.hStdOutput = output;

                SetErrorMode(ErrorModes.SEM_FAILCRITICALERRORS | ErrorModes.SEM_NOALIGNMENTFAULTEXCEPT |
                             ErrorModes.SEM_NOGPFAULTERRORBOX | ErrorModes.SEM_NOOPENFILEERRORBOX);

                const CreationFlags creationFlags = CreationFlags.CREATE_BREAKAWAY_FROM_JOB |
                                                    CreationFlags.CREATE_SUSPENDED |
                                                    CreationFlags.CREATE_SEPARATE_WOW_VDM |
                                                    CreationFlags.CREATE_NO_WINDOW;

                var file = Path.Combine(options.WorkingDirectory, options.Executable);
                if (!CreateProcess(null, file, ref securityAttributes,
                        ref securityAttributes, true, creationFlags, IntPtr.Zero, options.WorkingDirectory,
                        ref startupInfo, out var pi))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                try
                {
                    if (!AssignProcessToJobObject(job, pi.hProcess))
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    IntPtr[] lpHandles =
                    [
                        job,
                        pi.hProcess
                    ];
                    ResumeThread(pi.hThread);

                    var waitResult = WaitForMultipleObjects(2U, lpHandles, false,
                        (uint)options.TimeLimit.TotalMilliseconds);

                    var time = GetUserTimeConsumed(job);
                    var memory = GetPeakMemoryUsed(job);

                    if (waitResult == WAIT_FAILED)
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                    else if (waitResult == WAIT_TIMEOUT)
                    {
                        CloseHandle(job);
                        job = IntPtr.Zero;

                        return new RunResult
                        {
                            Status = RunStatus.TimeLimitExceeded,
                            ExitCode = -1,
                            TimeConsumedMilliseconds = time,
                            PeakMemoryUsed = memory
                        };
                    }

                    var result = GetQueuedCompletionStatus(ioCompletionPort);

                    if (!GetExitCodeProcess(pi.hProcess, out var exitCode))
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    return new RunResult
                    {
                        ExitCode = (int)exitCode,
                        Status = result,
                        TimeConsumedMilliseconds = time,
                        PeakMemoryUsed = memory
                    };
                }
                finally
                {
                    CloseHandle(pi.hThread);
                    CloseHandle(pi.hProcess);
                    CloseHandle(input);
                    CloseHandle(output);
                }
            }
            finally
            {
                CloseHandle(ioCompletionPort);
            }
        }
        finally
        {
            if (job != IntPtr.Zero)
                CloseHandle(job);
        }
    }

    private static int GetUserTimeConsumed(IntPtr job)
    {
        var lpJobObjectInfo = new JobObjectBasicAccountingInformation();

        var result = QueryInformationJobObject(job, JobObjectInfoType.BasicAccountingInformation,
            out lpJobObjectInfo, (uint)Marshal.SizeOf(lpJobObjectInfo), out _);

        return result
            ? (int)lpJobObjectInfo.TotalUserTime / 10000
            : throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    private static int GetPeakMemoryUsed(IntPtr job)
    {
        var lpJobObjectInfo = new JobobjectExtendedLimitInformation
        {
            BasicLimitInformation = new JobobjectBasicLimitInformation()
        };

        var result = QueryInformationJobObject(job, JobObjectInfoType.ExtendedLimitInformation,
            out lpJobObjectInfo,
            (uint)Marshal.SizeOf(lpJobObjectInfo), out _);

        return result
            ? (int)lpJobObjectInfo.PeakJobMemoryUsed
            : throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    private static RunStatus GetQueuedCompletionStatus(IntPtr ioCompletionPort)
    {
        while (Pinvoke.GetQueuedCompletionStatus(ioCompletionPort, out var code, out _, out _, 0U))
        {
            switch ((JobObjectMessageType)code)
            {
                case JobObjectMessageType.JOB_OBJECT_MSG_END_OF_PROCESS_TIME:
                case JobObjectMessageType.JOB_OBJECT_MSG_END_OF_JOB_TIME:
                    return RunStatus.TimeLimitExceeded;
                case JobObjectMessageType.JOB_OBJECT_MSG_ACTIVE_PROCESS_LIMIT:
                    return RunStatus.SecurityViolation;
                case JobObjectMessageType.JOB_OBJECT_MSG_ABNORMAL_EXIT_PROCESS:
                    return RunStatus.RuntimeError;
                case JobObjectMessageType.JOB_OBJECT_MSG_PROCESS_MEMORY_LIMIT:
                case JobObjectMessageType.JOB_OBJECT_MSG_JOB_MEMORY_LIMIT:
                    return RunStatus.MemoryLimitExceeded;
                default:
                    continue;
            }
        }

        return RunStatus.Success;
    }

    private static void AssociateCompletionPortInformation(IntPtr job, nint ioCompletionPort)
    {
        var completionPort = new JobObjectAssociateCompletionPort
        {
            CompletionKey = 1,
            CompletionPort = ioCompletionPort
        };

        SetInformationJobObject(job, completionPort, JobObjectInfoType.AssociateCompletionPortInformation);
    }

    private static void SetUiRestrictions(IntPtr job)
    {
        var uiRestrictions = new JobObjectBasicUIRestrictions
        {
            UIRestrictionsClass = UIRestrictionClass.JOB_OBJECT_UILIMIT_DESKTOP |
                                  UIRestrictionClass.JOB_OBJECT_UILIMIT_DISPLAYSETTINGS |
                                  UIRestrictionClass.JOB_OBJECT_UILIMIT_EXITWINDOWS |
                                  UIRestrictionClass.JOB_OBJECT_UILIMIT_GLOBALATOMS |
                                  UIRestrictionClass.JOB_OBJECT_UILIMIT_HANDLES |
                                  UIRestrictionClass.JOB_OBJECT_UILIMIT_READCLIPBOARD |
                                  UIRestrictionClass.JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS |
                                  UIRestrictionClass.JOB_OBJECT_UILIMIT_WRITECLIPBOARD
        };

        SetInformationJobObject(job, uiRestrictions, JobObjectInfoType.BasicUIRestrictions);
    }

    private static void SetExtendedInfo(IntPtr job, TimeSpan processorTimeLimit, int memoryLimit)
    {
        // Настраиваем ограничения
        var extendedInfo = new JobobjectExtendedLimitInformation
        {
            BasicLimitInformation = new JobobjectBasicLimitInformation
            {
                PerJobUserTimeLimit = processorTimeLimit.Ticks,
                LimitFlags = (ushort)(LimitFlags.JOB_OBJECT_LIMIT_JOB_TIME |
                                      LimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY |
                                      LimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE)
            },
            ProcessMemoryLimit = (UIntPtr)memoryLimit
        };

        SetInformationJobObject(job, extendedInfo, JobObjectInfoType.ExtendedLimitInformation);
    }

    private static void SetInformationJobObject<T>(IntPtr job, T lpJobObjectInfo3, JobObjectInfoType intoType)
        where T : struct
    {
        var length = Marshal.SizeOf(typeof(T));
        var extendedInfoPtr = Marshal.AllocHGlobal(length);

        try
        {
            Marshal.StructureToPtr(lpJobObjectInfo3, extendedInfoPtr, false);

            if (!Pinvoke.SetInformationJobObject(
                    job,
                    intoType,
                    extendedInfoPtr,
                    (uint)length))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        finally
        {
            Marshal.FreeHGlobal(extendedInfoPtr);
        }
    }
}