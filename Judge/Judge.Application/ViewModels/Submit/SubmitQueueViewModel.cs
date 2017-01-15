using System.Collections.Generic;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class SubmitQueueViewModel
    {
        public SubmitQueueViewModel(IEnumerable<SubmitQueueItem> submits)
        {
            Submits = submits;
        }

        public IEnumerable<SubmitQueueItem> Submits { get; private set; }
    }
}
