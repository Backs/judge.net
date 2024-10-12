using System.Collections.Generic;
using Judge.Model.CheckSolution;
using Judge.Model.Contests;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;

namespace Judge.Services.Converters;

internal interface ISubmitsConverter
{
    T Convert<T>(
        SubmitResult submitResult,
        Language language,
        Task task,
        User user,
        IReadOnlyCollection<ContestTask> contestTasks)
        where T : Web.Client.Submits.SubmitResultInfo, new();
}