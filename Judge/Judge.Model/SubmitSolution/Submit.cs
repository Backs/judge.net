namespace Judge.Model.SubmitSolution
{
    public sealed class Submit
    {
        public long UserId { get; set; }
        public long ProblemId { get; set; }
        public string FileName { get; set; }
        public int LanguageId { get; set; }
        public string SourceCode { get; set; }
        public long Id { get; set; }
    }
}
