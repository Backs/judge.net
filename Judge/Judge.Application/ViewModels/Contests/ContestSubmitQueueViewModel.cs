using System.Collections.Generic;
using Judge.Application.ViewModels.Submit;

namespace Judge.Application.ViewModels.Contests
{
    public sealed class ContestSubmitQueueViewModel : SubmitQueueViewModel
    {
        public ContestSubmitQueueViewModel(IEnumerable<SubmitQueueItem> submits) : base(submits)
        {
        }

        public override bool ShowProblem => false;
    }
}
