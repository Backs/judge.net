#nullable enable
using Judge.Model.SubmitSolution;

namespace Judge.JudgeService.Settings;

public class ProblemSettingsProvider : IProblemSettingsProvider
{
    private readonly CustomProblemSettings customProblemSettings;

    public ProblemSettingsProvider(CustomProblemSettings customProblemSettings)
    {
        this.customProblemSettings = customProblemSettings;
    }

    public ProblemSettings? GetProblemSettings(SubmitBase submit)
    {
        ProblemSettings? problemSettings = null;
        if (submit is ContestTaskSubmit contestTaskSubmit)
        {
            var contestSettings = this.customProblemSettings.Contests.TryGetValue(contestTaskSubmit.ContestId);
            problemSettings = contestSettings?.Problems.TryGetValue(contestTaskSubmit.ProblemId);
        }
        else if (submit is ProblemSubmit problemSubmit)
        {
            problemSettings = this.customProblemSettings.Problems.TryGetValue(problemSubmit.ProblemId);
        }

        return problemSettings;
    }
}