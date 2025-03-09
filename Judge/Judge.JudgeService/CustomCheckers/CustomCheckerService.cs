using System.Collections.Generic;
using System.Linq;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using FileOptions = Judge.Checker.FileOptions;

namespace Judge.JudgeService.CustomCheckers;

internal sealed class CustomCheckerService : ICustomCheckerService
{
    private readonly IProblemSettingsProvider problemSettingsProvider;
    private readonly IEnumerable<ICustomChecker> checkers;

    public CustomCheckerService(IProblemSettingsProvider problemSettingsProvider, IEnumerable<ICustomChecker> checkers)
    {
        this.problemSettingsProvider = problemSettingsProvider;
        this.checkers = checkers;
    }

    public ICollection<SubmitRunResult> Check(SubmitResult submitResult, FileOptions fileOptions,
        CheckerType checkerType)
    {
        var problemSettings = this.problemSettingsProvider.GetProblemSettings(submitResult.Submit);

        foreach (var checker in this.checkers.Where(o => o.Type == checkerType))
        {
            var result = checker.Check(problemSettings, submitResult, fileOptions);
            if (result != null)
                return result;
        }

        return null;
    }
}