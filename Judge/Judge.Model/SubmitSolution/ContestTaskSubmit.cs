namespace Judge.Model.SubmitSolution
{
    public sealed class ContestTaskSubmit : SubmitBase
    {
        public static ContestTaskSubmit Create()
        {
            var submit = new ContestTaskSubmit();
            submit.Results.Add(new SubmitResult(submit));
            return submit;
        }

        public int ContestId { get; set; }
    }
}
