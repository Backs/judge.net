using System.Collections.Generic;
using Judge.Model.SubmitSolution;
using Judge.Runner.Abstractions;
using FileOptions = Judge.Checker.FileOptions;

namespace Judge.JudgeService.CustomCheckers;

internal interface ICustomCheckerService
{
    ICollection<SubmitRunResult> Check(SubmitResult submitResult, FileOptions fileOptions, IRunResult runResult,
        CheckerType checkerType);
}