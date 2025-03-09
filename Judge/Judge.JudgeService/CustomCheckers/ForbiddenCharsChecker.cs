using System.Collections.Generic;
using System.Linq;
using Judge.Checker;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using Judge.Runner;

namespace Judge.JudgeService.CustomCheckers;

internal sealed class ForbiddenCharsChecker : ICustomChecker
{
    public CheckerType Type => CheckerType.PreExecutable;

    public ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult,
        FileOptions fileOptions)
    {
        if (problemSettings.ForbiddenChars != null &&
            submitResult.Submit.SourceCode.Any(o => problemSettings.ForbiddenChars.Contains(o)))
        {
            return new[]
            {
                new SubmitRunResult
                {
                    RunStatus = RunStatus.Success,
                    CheckStatus = CheckStatus.WA
                }
            };
        }

        return null;
    }
}