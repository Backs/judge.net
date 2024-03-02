using System.Collections.Generic;
using Judge.Model.SubmitSolution;

namespace Judge.JudgeService.CustomCheckers
{
    internal interface ICustomCheckerService
    {
        ICollection<SubmitRunResult> Check(SubmitResult submitResult);
    }
}