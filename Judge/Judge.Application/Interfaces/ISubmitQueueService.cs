using Judge.Application.ViewModels.Submit;

namespace Judge.Application.Interfaces
{
    public interface ISubmitQueueService
    {
        SubmitQueueViewModel GetSubmitQueue(long userId, long problemId, int page, int pageSize);
        SubmitQueueViewModel GetSubmitQueue(long? userId, int page, int pageSize);
    }
}
