using System.Collections.Generic;

namespace Judge.JudgeService.Settings
{
    public class ProblemSettings
    {
        public int? Language { get; set; }

        public string ForbiddenChars { get; set; }

        public string[] ForbiddenStrings { get; set; }

        public int? MaxSourceCodeLength { get; set; }
    }

    public class ContestSettings
    {
        public Dictionary<long, ProblemSettings> Problems { get; set; } = new Dictionary<long, ProblemSettings>();
    }


    public class CustomProblemSettings
    {
        public Dictionary<int, ContestSettings> Contests { get; set; } = new Dictionary<int, ContestSettings>();

        public Dictionary<long, ProblemSettings> Problems { get; set; } = new Dictionary<long, ProblemSettings>();

        public static CustomProblemSettings Empty { get; } = new CustomProblemSettings();
    }
}