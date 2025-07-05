using System.Collections.Generic;
using Judge.Checker;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using Judge.Runner.Abstractions;

namespace Judge.JudgeService.CustomCheckers;

internal sealed class MaxLengthChecker : ICustomChecker
{
    public CheckerType Type => CheckerType.PreExecutable;

    public ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult,
        FileOptions fileOptions)
    {
        if (problemSettings.MaxSourceCodeLength != null &&
            submitResult.Submit.SourceCode.Length > problemSettings.MaxSourceCodeLength)
        {
            return new[]
            {
                new SubmitRunResult
                {
                    RunStatus = RunStatus.Success,
                    CheckStatus = CheckStatus.PE
                }
            };
        }

        return null;
    }
}