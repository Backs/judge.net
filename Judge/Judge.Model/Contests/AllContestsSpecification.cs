using System;
using System.Linq.Expressions;

namespace Judge.Model.Contests;

public sealed class AllContestsSpecification : ISpecification<Contest>
{
    public AllContestsSpecification(bool openedOnly)
    {
        if (openedOnly)
        {
            IsSatisfiedBy = c => c.IsOpened;
        }
    }

    public Expression<Func<Contest, bool>> IsSatisfiedBy { get; } = a => true;
}