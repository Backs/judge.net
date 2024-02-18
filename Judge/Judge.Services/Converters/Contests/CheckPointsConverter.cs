using System.Collections.Generic;
using Judge.Model.Contests;
using Client = Judge.Web.Client.Contests;

namespace Judge.Services.Converters.Contests;

internal sealed class CheckPointsConverter : BaseContestConverter
{
    public static IContestConverter Instance { get; } = new CheckPointsConverter();

    protected override IComparer<Client.ContestUserResult> Comparer { get; } = new ContestUserResultComparer();

    protected override Client.ContestProblemResult ConvertContestTaskResult(Contest contest,
        ContestTaskResult contestTaskResult)
    {
        var diff = contestTaskResult.SubmitDateUtc.Subtract(contest.CheckPointTime!.Value).Duration();

        var points = 0;
        if (contestTaskResult.Solved)
        {
            points = (contestTaskResult.Attempts - 1) * 20 + (int)diff.TotalMinutes;
        }

        return new Client.ContestProblemResult
        {
            Points = points,
            Attempts = contestTaskResult.Attempts,
            Time = diff,
            Solved = contestTaskResult.Solved
        };
    }

    private sealed class ContestUserResultComparer : IComparer<Client.ContestUserResult>
    {
        public int Compare(Client.ContestUserResult? x, Client.ContestUserResult? y)
        {
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            if (x.SolvedCount == y.SolvedCount)
                return x.Points.CompareTo(y.Points);

            return y.SolvedCount.CompareTo(x.SolvedCount);
        }
    }
}