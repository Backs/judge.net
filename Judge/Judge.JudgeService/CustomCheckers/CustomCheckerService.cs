using System.Collections.Generic;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;

namespace Judge.JudgeService.CustomCheckers;

internal sealed class CustomCheckerService : ICustomCheckerService
{
    private readonly CustomProblemSettings customProblemSettings;
    private readonly IEnumerable<ICustomChecker> checkers;

    public CustomCheckerService(CustomProblemSettings customProblemSettings, IEnumerable<ICustomChecker> checkers)
    {
        this.customProblemSettings = customProblemSettings;
        this.checkers = checkers;
    }

    public ICollection<SubmitRunResult> Check(SubmitResult submitResult)
    {
        ProblemSettings problemSettings = null;
        if (submitResult.Submit is ContestTaskSubmit contestTaskSubmit)
        {
            var contestSettings = this.customProblemSettings.Contests.TryGetValue(contestTaskSubmit.ContestId);
            problemSettings = contestSettings?.Problems.TryGetValue(contestTaskSubmit.ProblemId);
        }
        else if (submitResult.Submit is ProblemSubmit problemSubmit)
        {
            problemSettings = this.customProblemSettings.Problems.TryGetValue(problemSubmit.ProblemId);
        }

        if (problemSettings == null)
            return null;

        foreach (var checker in this.checkers)
        {
            var result = checker.Check(problemSettings, submitResult);
            if (result != null)
                return result;
        }

        return null;
    }
}