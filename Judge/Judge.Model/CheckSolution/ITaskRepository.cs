namespace Judge.Model.CheckSolution
{
    public interface ITaskRepository
    {
        Task Get(long problemId);
    }
}
