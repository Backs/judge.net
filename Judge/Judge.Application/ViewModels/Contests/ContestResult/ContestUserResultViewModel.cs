using System.Collections.Generic;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class ContestUserResultViewModel
    {
        public long UserId { get; set; }
        public IReadOnlyCollection<ContestTaskResultViewModel> Tasks { get; set; }
        public string UserName { get; set; }
    }
}
