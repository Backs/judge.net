using System;
using System.Linq.Expressions;

namespace Judge.Model.Contests
{
    public sealed class OpenedContestsSpecification : ISpecification<Contest>
    {
        private OpenedContestsSpecification()
        {

        }

        public static OpenedContestsSpecification Instance { get; } = new OpenedContestsSpecification();

        public Expression<Func<Contest, bool>> IsSatisfiedBy { get; } = a => a.IsOpened;
    }
}
