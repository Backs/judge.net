using System;
using System.Linq.Expressions;

namespace Judge.Model.Contests;

public sealed class UpcomingContestSpecification : ISpecification<Contest>
{
    private readonly DateTime currentDate;

    public UpcomingContestSpecification(DateTime currentDate)
    {
        this.currentDate = currentDate;

        IsSatisfiedBy = contest => contest.FinishTime > this.currentDate && contest.IsOpened;
    }

    public Expression<Func<Contest, bool>> IsSatisfiedBy { get; }
}