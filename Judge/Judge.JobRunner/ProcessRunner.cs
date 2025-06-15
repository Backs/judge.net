using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
            var extendedInfo = new Invoke.JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = new Invoke.JOBOBJECT_BASIC_LIMIT_INFORMATION
                {
                    PerJobUserTimeLimit = timeLimit.Ticks,
                    LimitFlags = (ushort)(Invoke.LimitFlags.JOB_OBJECT_LIMIT_JOB_TIME |
                                          Invoke.LimitFlags.JOB_OBJECT_LIMIT_PROCESS_MEMORY |
                                          Invoke.LimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE)
                },
                ProcessMemoryLimit = (UIntPtr)memoryLimitKiloBytes
            };

            var length = Marshal.SizeOf(typeof(Invoke.JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
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

            var peakWorkingSet64 = 0L;
            var processPeakPagedMemorySize64 = 0L;
            var peakVirtualMemorySize64 = 0L;
            var task = Task.Run<Task>(async () =>
            {
                while (!process.HasExited)
                {
                    peakWorkingSet64 = process.PeakWorkingSet64;
                    processPeakPagedMemorySize64 = process.PeakPagedMemorySize64;
                    peakVirtualMemorySize64 = process.PeakVirtualMemorySize64;
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
            });

            if (!Invoke.AssignProcessToJobObject(hJob, process.Handle))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            input?.Invoke(process.StandardInput);

            if (!process.WaitForExit((int)timeLimit.TotalMilliseconds))
            {
                process.Kill();
                return new RunResult
                {
                    ExitCode = process.ExitCode,
                    Status = RunStatus.TimeLimitExceeded
                };
            }

            if (process.ExitCode != 0)
            {
                return new RunResult
                {
                    ExitCode = process.ExitCode,
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