namespace Judge.Model.SubmitSolution
{
    public interface ISubmitRepository
    {
        void Add(ProblemSubmit item);
        ProblemSubmit Get(long submitId);
    }
}
