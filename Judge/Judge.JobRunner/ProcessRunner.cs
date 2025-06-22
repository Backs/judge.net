using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Judge.JobRunner;

public static class ProcessRunner
{
    public static RunResult RunProcessWithLimits(
        string executable,
        string arguments,
        TimeSpan timeLimit,
        int memoryLimitKiloBytes,
        Action<StreamWriter>? input = null)
    {
        var hJob = Invoke.CreateJobObject(IntPtr.Zero, null);
        if (hJob == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        try
        {
            // Настраиваем ограничения
            var extendedInfo = new Invoke.JobobjectExtendedLimitInformation
            {
                BasicLimitInformation = new Invoke.JobobjectBasicLimitInformation
                {
                    PerJobUserTimeLimit = timeLimit.Ticks,
                    LimitFlags = (ushort)(Invoke.LimitFlags.JOB_OBJECT_LIMIT_JOB_TIME |
                                          Invoke.LimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY |
                                          Invoke.LimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE)
                },
                ProcessMemoryLimit = (UIntPtr)memoryLimitKiloBytes
            };

            var length = Marshal.SizeOf(typeof(Invoke.JobobjectExtendedLimitInformation));
            var extendedInfoPtr = Marshal.AllocHGlobal(length);

            try
            {
                Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

                if (!Invoke.SetInformationJobObject(
                        hJob,
                        Invoke.JobObjectInfoType.ExtendedLimitInformation,
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

            var ioCompletionPort = Invoke.CreateIoCompletionPort(-1, IntPtr.Zero, 1U, 1U);
            if (ioCompletionPort == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            var completionPortJobInfo = new Invoke.JobObjectAssociateCompletionPort
            {
                CompletionKey = 1,
                CompletionPort = ioCompletionPort
            };

            var portInfoPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(completionPortJobInfo, portInfoPtr, false);

            if (!Invoke.SetInformationJobObject(hJob, Invoke.JobObjectInfoType.AssociateCompletionPortInformation,
                    portInfoPtr, (uint)Marshal.SizeOf(typeof(Invoke.JobObjectAssociateCompletionPort))))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = executable,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };

            var output = new StringBuilder();
            process.OutputDataReceived += (_, args) => output.Append(args.Data);
            process.Start();
            process.BeginOutputReadLine();

            if (!Invoke.AssignProcessToJobObject(hJob, process.Handle))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            input?.Invoke(process.StandardInput);

            IntPtr[] lpHandles =
            [
                hJob,
                process.Handle
            ];

            Invoke.WaitForMultipleObjects(2U, lpHandles, true, (uint)timeLimit.TotalMilliseconds);

            var completionStatus = RetrieveQueuedCompletionStatus(ioCompletionPort);

            if (completionStatus != RunStatus.Success)
            {
                Console.WriteLine("CS");
                return new RunResult
                {
                    Status = completionStatus,
                    ExitCode = process.ExitCode
                };
            }

            if (!Invoke.GetExitCodeProcess(process.Handle, out var exitCode))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            var time = GetUserTimeConsumed(hJob);

            if (time > timeLimit)
            {
                Console.WriteLine("TL");
                return new RunResult
                {
                    ExitCode = (int)exitCode,
                    Status = RunStatus.TimeLimitExceeded
                };
            }

            if (exitCode != 0)
            {
                Console.WriteLine("EXIT CODE");
                return new RunResult
                {
                    ExitCode = (int)exitCode,
                    Status = RunStatus.RuntimeError
                };
            }

            return new RunResult
            {
                ExitCode = process.ExitCode,
                Status = RunStatus.Success,
                Output = output.ToString()
            };
        }
        finally
        {
            Invoke.CloseHandle(hJob);
        }
    }

    private static RunStatus RetrieveQueuedCompletionStatus(IntPtr ioCompletionPort)
    {
        while (Invoke.GetQueuedCompletionStatus(ioCompletionPort, out var code, out _, out _, 0U))
        {
            Console.WriteLine((Invoke.JobObjectMessageType)code);
            switch ((Invoke.JobObjectMessageType)code)
            {
                case Invoke.JobObjectMessageType.JOB_OBJECT_MSG_END_OF_PROCESS_TIME:
                case Invoke.JobObjectMessageType.JOB_OBJECT_MSG_END_OF_JOB_TIME:
                    return RunStatus.TimeLimitExceeded;
                case Invoke.JobObjectMessageType.JOB_OBJECT_MSG_ACTIVE_PROCESS_LIMIT:
                    return RunStatus.SecurityViolation;
                case Invoke.JobObjectMessageType.JOB_OBJECT_MSG_ABNORMAL_EXIT_PROCESS:
                    return RunStatus.RuntimeError;
                case Invoke.JobObjectMessageType.JOB_OBJECT_MSG_JOB_MEMORY_LIMIT:
                case Invoke.JobObjectMessageType.JOB_OBJECT_MSG_PROCESS_MEMORY_LIMIT:
                    return RunStatus.MemoryLimitExceeded;
                default:
                    continue;
            }
        }

        return RunStatus.Success;
    }

    private static TimeSpan GetUserTimeConsumed(IntPtr job)
    {
        var basicAccountingInformation =
            QueryJobInformation<Invoke.JobObjectBasicAccountingInformation>(job,
                Invoke.JobObjectInfoType.BasicAccountingInformation);

        if (basicAccountingInformation == null)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        return new TimeSpan(0, 0, 0, 0, (int)basicAccountingInformation.Value.TotalUserTime / 10000);
    }

    static T? QueryJobInformation<T>(IntPtr hJob, Invoke.JobObjectInfoType infoType) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        IntPtr infoPtr = Marshal.AllocHGlobal(size);

        try
        {
            if (Invoke.QueryInformationJobObject(hJob, infoType, infoPtr, (uint)size, out _))
            {
                return Marshal.PtrToStructure<T>(infoPtr);
            }

            return null;
        }
        finally
        {
            Marshal.FreeHGlobal(infoPtr);
        }
    }
}