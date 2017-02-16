using Judge.Model.SubmitSolution;

namespace Judge.JudgeService
{
    internal interface IJudgeService
    {
        JudgeResult Check(SubmitResult submitResult);
    }
}
