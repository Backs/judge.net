using System;
using System.Globalization;
using System.Linq;
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
        public string Output { get; private set; }

        private RunResult()
        {

        }

        private static readonly Regex TimeRegex = new Regex(@"\d*\.\d{2}", RegexOptions.Compiled);
        private static readonly Regex NumberRegex = new Regex(@"\d+", RegexOptions.Compiled);

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
            var exitCode = rows.FirstOrDefault(o => o.StartsWith("  exit code"));

            var runResult = new RunResult
            {
                TimeConsumedMilliseconds = (int)(double.Parse(TimeRegex.Match(timeConsumed).Value, CultureInfo.InvariantCulture) * 1000),
                TimePassedMilliseconds = (int)(double.Parse(TimeRegex.Match(timePassed).Value, CultureInfo.InvariantCulture) * 1000),
                PeakMemoryBytes = int.Parse(NumberRegex.Match(peakMemory).Value),
                RunStatus = GetStatus(textStatus, exitCode != null ? int.Parse(NumberRegex.Match(exitCode).Value) : 0),
                TextStatus = textStatus,
                Description = description,
                Output = input
            };
            return runResult;
        }

        private static RunStatus GetStatus(string textStatus, int exitCode)
        {
            if (exitCode != 0)
            {
                return RunStatus.RuntimeError;
            }

            switch (textStatus)
            {
                case "Time limit exceeded":
                    return RunStatus.TimeLimitExceeded;
                case "Memory limit exceeded":
                    return RunStatus.MemoryLimitExceeded;
                case "Crash":
                    return RunStatus.RuntimeError;
            }
            if (textStatus.StartsWith("Security violation"))
                return RunStatus.SecurityViolation;

            throw new ArgumentOutOfRangeException(nameof(textStatus));
        }
    }
}
