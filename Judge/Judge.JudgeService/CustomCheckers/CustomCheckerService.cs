using System.Collections.Generic;
using System.Linq;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using Judge.Runner.Abstractions;
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
        IRunResult runResult,
        CheckerType checkerType)
    {
        var problemSettings = this.problemSettingsProvider.GetProblemSettings(submitResult.Submit);
        if (problemSettings == null)
            return null;

        foreach (var checker in this.checkers.Where(o => o.Type == checkerType))
        {
            var result = checker.Check(problemSettings, submitResult, fileOptions, runResult);
            if (result != null)
                return result;
        }

        return null;
    }
}