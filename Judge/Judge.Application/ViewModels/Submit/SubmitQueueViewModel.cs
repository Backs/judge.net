using System.Collections.Generic;

namespace Judge.Application.ViewModels.Submit
{
    public class SubmitQueueViewModel
    {
        public SubmitQueueViewModel(ICollection<SubmitQueueItem> submits)
        {
            Submits = submits;
        }

        public ICollection<SubmitQueueItem> Submits { get; }
        public PaginationViewModel Pagination { get; set; }
        public virtual bool ShowProblem => true;
    }
}
