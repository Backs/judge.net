using System;
using System.Collections.Generic;
using System.Linq;
using Judge.Checker;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using Judge.Runner;

namespace Judge.JudgeService.CustomCheckers
{
    internal sealed class ForbiddenStringsChecker : ICustomChecker
    {
        public ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult)
        {
            if (problemSettings.ForbiddenStrings != null &&
                problemSettings.ForbiddenStrings.Any(o =>
                    submitResult.Submit.SourceCode.IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0))
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
}