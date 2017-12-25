namespace Judge.Application.ViewModels.Problems.Solution
{
    public class SolutionViewModel
    {
        public string ProblemName { get; set; }
        public long ProblemId { get; set; }
        public string SourceCode { get; set; }
        public SubmitViewModel SubmitResults { get; set; }
    }
}