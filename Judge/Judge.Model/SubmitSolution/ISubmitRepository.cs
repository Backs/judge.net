namespace Judge.Model.SubmitSolution
{
    public interface ISubmitRepository
    {
        void Add(SubmitBase item);
        SubmitBase Get(long submitId);
    }
}
