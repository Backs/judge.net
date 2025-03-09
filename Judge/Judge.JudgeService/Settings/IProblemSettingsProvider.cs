#nullable enable
using Judge.Model.SubmitSolution;

namespace Judge.JudgeService.Settings;

public interface IProblemSettingsProvider
{
    ProblemSettings? GetProblemSettings(SubmitBase submit);
}