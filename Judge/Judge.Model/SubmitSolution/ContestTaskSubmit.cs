namespace Judge.Model.SubmitSolution;

public sealed class ContestTaskSubmit : SubmitBase
{
    public int ContestId { get; set; }

    public static ContestTaskSubmit Create()
    {
        var submit = new ContestTaskSubmit();
        submit.Results.Add(new SubmitResult(submit));
        return submit;
    }
}