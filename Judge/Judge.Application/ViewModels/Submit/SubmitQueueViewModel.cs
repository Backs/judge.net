using System.Collections.Generic;

namespace Judge.Application.ViewModels.Submit
{
    public class SubmitQueueViewModel
    {
        public SubmitQueueViewModel(IEnumerable<SubmitQueueItem> submits)
        {
            Submits = submits;
        }

        public IEnumerable<SubmitQueueItem> Submits { get; }
        public PaginationViewModel Pagination { get; set; }
        public virtual bool ShowProblem => true;
    }
}
