using System.Collections.Generic;

namespace Judge.Runner
{
    public sealed class Configuration
    {
        public string FileName { get; set; }
        public int TimeLimitMilliseconds { get; set; }
        public int MemoryLimitBytes { get; set; }
        public string Directory { get; set; }
        public bool Quiet { get; set; }
        public bool ShowWindow { get; set; }
        public bool SingleCore { get; set; }
        public bool AllowCreateProcesses { get; set; }

        public Configuration(string fileName, string directory, int timeLimitMilliseconds, int memoryLimitBytes)
        {
            TimeLimitMilliseconds = timeLimitMilliseconds;
            MemoryLimitBytes = memoryLimitBytes;
            FileName = fileName;
            Directory = directory;
            Quiet = false;
            ShowWindow = false;
            SingleCore = true;
            AllowCreateProcesses = false;
        }

        public Configuration()
        {

        }

        public override string ToString()
        {
            var arguments = new List<string>(16);

            arguments.Add("-t");
            arguments.Add(TimeLimitMilliseconds + "ms");

            arguments.Add("-m");
            arguments.Add(MemoryLimitBytes / 1024 + "K");

            if (!string.IsNullOrWhiteSpace(Directory))
            {
                arguments.Add("-d");
                arguments.Add($"\"{Directory}\"");
            }

            if (Quiet)
            {
                arguments.Add("-q");
            }

            if (ShowWindow)
            {
                arguments.Add("-w");
            }

            if (SingleCore)
            {
                arguments.Add("-1");
            }

            if (AllowCreateProcesses)
            {
                arguments.Add("-Xacp");
            }

            arguments.Add($"\"{FileName}\"");

            return string.Join(" ", arguments);
        }
    }
}
