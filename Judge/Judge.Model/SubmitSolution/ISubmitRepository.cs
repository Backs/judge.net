namespace Judge.Model.SubmitSolution
{
    public interface ISubmitRepository
    {
        void Add(SubmitBase item);
        ProblemSubmit Get(long submitId);
    }
}
