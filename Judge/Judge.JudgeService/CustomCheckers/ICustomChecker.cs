using System.Collections.Generic;
using Judge.JudgeService.Settings;
using Judge.Model.SubmitSolution;
using FileOptions = Judge.Checker.FileOptions;

namespace Judge.JudgeService.CustomCheckers;

internal interface ICustomChecker
{
    CheckerType Type { get; }
    ICollection<SubmitRunResult> Check(ProblemSettings problemSettings, SubmitResult submitResult,
        FileOptions fileOptions);
}