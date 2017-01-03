using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Judge.Runner
{
    public sealed class RunResult
    {
        public RunStatus RunStatus { get; private set; }
        public int TimeConsumedMilliseconds { get; private set; }
        public int TimePassedMilliseconds { get; private set; }
        public int PeakMemoryBytes { get; private set; }
        public string TextStatus { get; private set; }
        public string Description { get; private set; }

        private RunResult()
        {

        }

        private static readonly Regex TimeRegex = new Regex(@"\d*\.\d{2}", RegexOptions.Compiled);
        private static readonly Regex MemoryRegex = new Regex(@"\d+", RegexOptions.Compiled);

        public static RunResult Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentOutOfRangeException(nameof(input));
            }

            var rows = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var startIndex = Array.FindIndex(rows, o => o.StartsWith("  time consumed"));

            var textStatus = rows[1];
            var description = startIndex == 2 ? rows[1] : rows[2];
            var timeConsumed = rows[startIndex];
            var timePassed = rows[startIndex + 1];
            var peakMemory = rows[startIndex + 2];

            var runResult = new RunResult
            {
                TimeConsumedMilliseconds = (int)(double.Parse(TimeRegex.Match(timeConsumed).Value, CultureInfo.InvariantCulture) * 1000),
                TimePassedMilliseconds = (int)(double.Parse(TimeRegex.Match(timePassed).Value, CultureInfo.InvariantCulture) * 1000),
                PeakMemoryBytes = int.Parse(MemoryRegex.Match(peakMemory).Value),
                RunStatus = GetStatus(textStatus),
                TextStatus = textStatus,
                Description = description
            };
            return runResult;
        }

        private static RunStatus GetStatus(string textStatus)
        {
            switch (textStatus)
            {
                case "Time limit exceeded":
                    return RunStatus.TimeLimitExceeded;
                case "Memory limit exceeded":
                    return RunStatus.MemoryLimitExceeded;
            }
            if (textStatus.StartsWith("Security violation"))
                return RunStatus.SecurityViolation;

            throw new ArgumentOutOfRangeException(nameof(textStatus));
        }
    }
}
