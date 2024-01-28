namespace Judge.Web.Client.Submits;

public sealed class SubmitResultExtendedInfo : SubmitResultInfo
{
    public string SourceCode { get; set; } = null!;
    public string? CompilerOutput { get; set; }
    public string? RunOutput { get; set; }
    public string? UserHost { get; set; }
}