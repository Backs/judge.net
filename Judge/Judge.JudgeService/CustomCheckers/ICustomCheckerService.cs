using System.Collections.Generic;
using Judge.Model.SubmitSolution;
using FileOptions = Judge.Checker.FileOptions;

namespace Judge.JudgeService.CustomCheckers;

internal interface ICustomCheckerService
{
    ICollection<SubmitRunResult> Check(SubmitResult submitResult, FileOptions fileOptions, CheckerType checkerType);
}