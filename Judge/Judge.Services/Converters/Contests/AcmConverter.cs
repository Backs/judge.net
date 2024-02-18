using System.Collections.Generic;
using Judge.Model.Contests;
using Client = Judge.Web.Client.Contests;

namespace Judge.Services.Converters.Contests;

internal sealed class AcmConverter : BaseContestConverter
{
    public static IContestConverter Instance { get; } = new AcmConverter();

    protected override IComparer<Client.ContestUserResult> Comparer { get; } = new ContestTaskResultComparer();

    protected override Client.ContestProblemResult ConvertContestTaskResult(Contest contest,
        ContestTaskResult contestTaskResult)
    {
        var elapsedTime = contestTaskResult.SubmitDateUtc - contest.StartTime;
        var points = 0;
        if (contestTaskResult.Solved)
        {
            points = (contestTaskResult.Attempts - 1) * 20 + (int)elapsedTime.TotalMinutes;
        }

        return new Client.ContestProblemResult
        {
            Solved = contestTaskResult.Solved,
            Points = points,
            Attempts = contestTaskResult.Attempts,
            Time = elapsedTime
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

            if (x.SolvedCount == y.SolvedCount)
                return x.Points.CompareTo(y.Points);

            return y.SolvedCount.CompareTo(x.SolvedCount);
        }
    }
}