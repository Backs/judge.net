using Client = Judge.Web.Client.Contests;

using System.Collections.Generic;
using Judge.Model.Contests;
using Judge.Model.Entities;

namespace Judge.Services.Converters.Contests;

public interface IContestConverter
{
    Client.ContestUserResult[] Convert(Contest contest,
        IReadOnlyDictionary<long, ContestTask> contestTasks,
        IReadOnlyCollection<ContestResult> contestResults,
        IReadOnlyDictionary<long, User> users);
}