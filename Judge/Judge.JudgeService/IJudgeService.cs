using Judge.Model.SubmitSolution;

namespace Judge.JudgeService
{
    internal interface IJudgeService
    {
        void Check(SubmitResult submitResult);
    }
}
