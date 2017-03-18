namespace Judge.Model.SubmitSolution
{
    public interface ISubmitRepository
    {
        void Add(Submit item);
        Submit Get(long submitId);
    }
}
