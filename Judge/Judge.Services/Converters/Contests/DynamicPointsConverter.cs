using System.Collections.Generic;
using System.Linq;
using Judge.Model.Contests;
using Contest = Judge.Model.Contests.Contest;
using Client = Judge.Web.Client.Contests;

namespace Judge.Services.Converters.Contests;

internal sealed class DynamicPointsConverter : BaseContestConverter
{
    public static IContestConverter Instance { get; } = new DynamicPointsConverter();
    protected override IComparer<Client.ContestUserResult> Comparer { get; } = new ContestTaskResultComparer();

    protected override Client.ContestProblemResult ConvertContestTaskResult(Contest contest,
        ContestTaskResult contestTaskResult, IReadOnlyCollection<ContestResult> allResults)
    {
        var solvedCount = allResults.Count(o =>
            o.TaskResults.Any(t => t.ProblemId == contestTaskResult.ProblemId && t.Solved));

        var points = 0;
        if (contestTaskResult.Solved)
        {
            points = MaxScore / solvedCount - (contestTaskResult.Attempts - 1) * Penalty;
            points = points <= 0 ? 10 : points;
        }

        return new Client.ContestProblemResult
        {
            Points = points,
            Attempts = contestTaskResult.Attempts,
            Solved = contestTaskResult.Solved
        };
    }

    private const int Penalty = 20;
    private const int MaxScore = 1000;

    private sealed class ContestTaskResultComparer : IComparer<Client.ContestUserResult>
    {
        public int Compare(Client.ContestUserResult? x, Client.ContestUserResult? y)
        {
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            return y.Points.CompareTo(x.Points);
        }
    }
}