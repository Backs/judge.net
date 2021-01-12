namespace Judge.Application.Interfaces
{
    using Judge.Application.ViewModels.Submit;

    public interface ISubmitQueueService
    {
        SubmitQueueViewModel GetSubmitQueue(long userId, long problemId, int page, int pageSize);

        SubmitQueueViewModel GetSubmitQueue(long? userId, int page, int pageSize);
    }
}
