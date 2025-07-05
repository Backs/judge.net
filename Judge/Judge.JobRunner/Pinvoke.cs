using System;
using System.Runtime.InteropServices;

namespace Judge.JobRunner;

public static class Pinvoke
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string? lpName);

    [DllImport("kernel32.dll")]
    public static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType,
        IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

    [DllImport("kernel32.dll")]
    public static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool IsProcessInJob(IntPtr Process, IntPtr Job, out bool Result);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool QueryInformationJobObject(
        IntPtr hJob,
        JobObjectInfoType infoType,
        out JobObjectBasicAccountingInformation lpJobObjectInfo,
        uint cbJobObjectInfoLength,
        out uint lpReturnLength);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool QueryInformationJobObject(
        IntPtr hJob,
        JobObjectInfoType infoType,
        out JobobjectExtendedLimitInformation lpJobObjectInfo,
        uint cbJobObjectInfoLength,
        out uint lpReturnLength);

    [DllImport("kernel32.dll")]
    public static extern bool GetQueuedCompletionStatus(
        IntPtr completionPort,
        out uint code,
        out UIntPtr lpCompletionKey,
        out IntPtr lpOverlapped,
        uint dwMilliseconds);

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateIoCompletionPort(
        IntPtr fileHandle,
        IntPtr existingCompletionPort,
        UIntPtr completionKey,
        uint numberOfConcurrentThreads);

    public const uint WAIT_TIMEOUT = 0x102;
    public const uint WAIT_FAILED = 0xFFFFFFFF;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint WaitForMultipleObjects(
        uint nCount,
        IntPtr[] lpHandles,
        bool bWaitAll,
        uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetExitCodeProcess(IntPtr process, out uint exitCode);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern ErrorModes SetErrorMode(ErrorModes uMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CreateProcess(
        string? imageName,
        string cmdLine,
        ref SecurityAttributes lpProcessAttributes,
        ref SecurityAttributes lpThreadAttributes,
        bool boolInheritHandles,
        CreationFlags dwCreationFlags,
        IntPtr lpEnvironment,
        string lpszCurrentDir,
        ref StartupInfo startupInfo,
        out ProcessInformation pi);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint ResumeThread(IntPtr hThread);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFile(string lpFileName, DesiredAccess dwDesiredAccess, uint dwShareMode,
        ref SecurityAttributes lpSecurityAttributes, CreationDisposition dwCreationDisposition,
        uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    public enum DesiredAccess : uint
    {
        GENERIC_WRITE = 30,
        GENERIC_READ = 31
    }

    public enum CreationDisposition : uint
    {
        CREATE_NEW = 1,
        CREATE_ALWAYS = 2,
        OPEN_EXISTING = 3,
        OPEN_ALWAYS = 4,
        TRUNCATE_EXISTING = 5
    }

    [Flags]
    public enum ErrorModes : uint
    {
        SYSTEM_DEFAULT = 0,
        SEM_FAILCRITICALERRORS = 1,
        SEM_NOALIGNMENTFAULTEXCEPT = 4,
        SEM_NOGPFAULTERRORBOX = 2,
        SEM_NOOPENFILEERRORBOX = 32768, // 0x00008000
    }

    [Flags]
    public enum CreationFlags
    {
        CREATE_BREAKAWAY_FROM_JOB = 16777216, // 0x01000000
        CREATE_SUSPENDED = 4,
        CREATE_NEW_CONSOLE = 16, // 0x00000010
        CREATE_NEW_PROCESS_GROUP = 512, // 0x00000200
        CREATE_UNICODE_ENVIRONMENT = 1024, // 0x00000400
        CREATE_SEPARATE_WOW_VDM = 2048, // 0x00000800
        CREATE_DEFAULT_ERROR_MODE = 67108864, // 0x04000000
        CREATE_NO_WINDOW = 134217728, // 0x08000000
        CREATE_PROTECTED_PROCESS = 262144, // 0x00040000
        DEBUG_PROCESS = 1,
        DEBUG_ONLY_THIS_PROCESS = 2,
    }

    public enum JobObjectInfoType
    {
        BasicAccountingInformation = 1,
        AssociateCompletionPortInformation = 7,
        BasicLimitInformation = 2,
        BasicUIRestrictions = 4,
        EndOfJobTimeInformation = 6,
        ExtendedLimitInformation = 9,
        SecurityLimitInformation = 5,
        GroupInformation = 11
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessInformation
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityAttributes
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JobObjectBasicAccountingInformation
    {
        public ulong TotalUserTime;
        public ulong TotalKernelTime;
        public ulong ThisPeriodTotalUserTime;
        public ulong ThisPeriodTotalKernelTime;
        public uint TotalPageFaultCount;
        public uint TotalProcesses;
        public uint ActiveProcesses;
        public uint TotalTerminatedProcesses;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JobobjectBasicLimitInformation
    {
        public long PerProcessUserTimeLimit;
        public long PerJobUserTimeLimit;
        public ushort LimitFlags;
        public UIntPtr MinimumWorkingSetSize;
        public UIntPtr MaximumWorkingSetSize;
        public ushort ActiveProcessLimit;
        public long Affinity;
        public ushort PriorityClass;
        public ushort SchedulingClass;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IoCounters
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JobobjectExtendedLimitInformation
    {
        public JobobjectBasicLimitInformation BasicLimitInformation;
        public IoCounters IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JobObjectAssociateCompletionPort
    {
        public int CompletionKey;
        public IntPtr CompletionPort;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JobObjectBasicUIRestrictions
    {
        public UIRestrictionClass UIRestrictionsClass;
    }

    [Flags]
    public enum UIRestrictionClass : uint
    {
        JOB_OBJECT_UILIMIT_DESKTOP = 64, // 0x00000040
        JOB_OBJECT_UILIMIT_DISPLAYSETTINGS = 16, // 0x00000010
        JOB_OBJECT_UILIMIT_EXITWINDOWS = 128, // 0x00000080
        JOB_OBJECT_UILIMIT_GLOBALATOMS = 32, // 0x00000020
        JOB_OBJECT_UILIMIT_HANDLES = 1,
        JOB_OBJECT_UILIMIT_READCLIPBOARD = 2,
        JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS = 8,
        JOB_OBJECT_UILIMIT_WRITECLIPBOARD = 4,
    }

    public enum JobObjectMessageType : uint
    {
        JOB_OBJECT_MSG_END_OF_JOB_TIME = 1,
        JOB_OBJECT_MSG_END_OF_PROCESS_TIME = 2,
        JOB_OBJECT_MSG_ACTIVE_PROCESS_LIMIT = 3,
        JOB_OBJECT_MSG_ACTIVE_PROCESS_ZERO = 4,
        JOB_OBJECT_MSG_NEW_PROCESS = 6,
        JOB_OBJECT_MSG_EXIT_PROCESS = 7,
        JOB_OBJECT_MSG_ABNORMAL_EXIT_PROCESS = 8,
        JOB_OBJECT_MSG_PROCESS_MEMORY_LIMIT = 9,
        JOB_OBJECT_MSG_JOB_MEMORY_LIMIT = 10, // 0x0000000A
    }

    [Flags]
    public enum LimitFlags : ushort
    {
        JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001,
        JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002,
        JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004,
        JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008,
        JOB_OBJECT_LIMIT_AFFINITY = 0x00000010,
        JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020,
        JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040,
        JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080,
        JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100,
        JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200,
        JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400,
        JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800,
        JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000,
        JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000
    }

    public const int STILL_ACTIVE = 259;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct StartupInfo
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }
}