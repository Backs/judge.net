using Judge.Application.ViewModels.Submit;

namespace Judge.Application.Interfaces
{
    public interface ISubmitQueueService
    {
        SubmitQueueViewModel GetSubmitQueue(long? userId);
    }
}
