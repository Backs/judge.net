using System.Collections.Generic;
using Judge.Model.Contests;
using Client = Judge.Web.Client.Contests;

namespace Judge.Services.Converters.Contests;

internal sealed class PointsConverter : BaseContestConverter
{
    private const int MaxScore = 100;
    private const int Penalty = 20;
    private const int MinScore = 5;
    public static IContestConverter Instance { get; } = new PointsConverter();

    protected override IComparer<Client.ContestUserResult> Comparer { get; } = new ContestTaskResultComparer();

    protected override Client.ContestProblemResult ConvertContestTaskResult(Contest contest,
        ContestTaskResult contestTaskResult, IReadOnlyCollection<ContestResult> allResults)
    {
        var points = 0;
        if (contestTaskResult.Solved)
        {
            points = MaxScore - (contestTaskResult.Attempts - 1) * Penalty;
            points = points <= 0 ? MinScore : points;
        }

        return new Client.ContestProblemResult
        {
            Points = points,
            Attempts = contestTaskResult.Attempts,
            Solved = contestTaskResult.Solved
        };
    }

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