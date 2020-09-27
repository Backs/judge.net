using System.Collections.Generic;
using Judge.Model.Contests;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    internal interface IContestTaskResultFactory
    {
        ContestResultViewModel Convert(
            IEnumerable<ContestTask> tasks,
            IEnumerable<Model.Contests.ContestResult> results,
            IDictionary<long, string> users,
            Contest contest);
    }
}
