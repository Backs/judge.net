using System;
using System.Collections.Generic;
using System.Linq;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Client = Judge.Web.Client.Contests;

namespace Judge.Services.Converters.Contests;

public abstract class BaseContestConverter : IContestConverter
{
    protected abstract IComparer<Client.ContestUserResult> Comparer { get; }

    public Client.ContestUserResult[] Convert(
        Contest contest,
        IReadOnlyDictionary<long, ContestTask> contestTasks,
        IReadOnlyCollection<ContestResult> contestResults,
        IReadOnlyDictionary<long, User> users)
    {
        var result = contestResults.Select(o => new Client.ContestUserResult
            {
                UserId = o.UserId,
                UserName = users[o.UserId].UserName,
                SolvedCount = o.TaskResults.Count(t => t.Solved),
                Tasks = o.TaskResults.Select(t => new
                    {
                        Label = contestTasks[t.ProblemId].TaskName,
                        Result = this.ConvertContestTaskResult(contest, t)
                    }).OrderBy(t => t.Label)
                    .ToDictionary(t => t.Label, t => t.Result),
            })
            .ToArray();

        foreach (var item in result)
        {
            item.Points = item.Tasks.Values.Sum(o => o.Points);
        }

        Array.Sort(result, this.Comparer);

        var position = 1;
        if (result.Length != 0)
        {
            result[0].Position = position;
        }

        for (var i = 1; i < result.Length; i++)
        {
            if (this.Comparer.Compare(result[i], result[i - 1]) != 0)
            {
                position++;
            }

            result[i].Position = position;
        }

        return result;
    }

    protected abstract Client.ContestTaskResult ConvertContestTaskResult(Contest contest,
        ContestTaskResult contestTaskResult);
}