using Judge.Checker;
using Judge.Runner.Abstractions;

namespace Judge.JudgeService;

internal sealed class SubmitRunResult
{
    public RunStatus RunStatus { get; set; }
    public int TimeConsumedMilliseconds { get; set; }
    public int TimePassedMilliseconds { get; set; }
    public int PeakMemoryBytes { get; set; }
    public string TextStatus { get; set; }
    public string Description { get; set; }
    public string Output { get; set; }
    public CheckStatus CheckStatus { get; set; }
    public string CheckMessage { get; set; }

    public bool RunSuccess => this.RunStatus == RunStatus.Success && this.CheckStatus == CheckStatus.OK;
}