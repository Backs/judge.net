namespace Judge.Application.ViewModels.Contests.ContestResult
{
    using System.Collections.Generic;
    using Judge.Model.Contests;
    using ContestResult = Judge.Model.Contests.ContestResult;

    internal interface IContestTaskResultFactory
    {
        ContestResultViewModel Convert(IEnumerable<ContestTask> tasks,
        IReadOnlyList<ContestResult> results,
        IDictionary<long, string> users,
        Contest contest);
    }
}
