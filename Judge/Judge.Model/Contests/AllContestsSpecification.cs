using System;
using System.Linq.Expressions;

namespace Judge.Model.Contests
{
    public sealed class AllContestsSpecification : ISpecification<Contest>
    {
        private AllContestsSpecification()
        {

        }

        public static AllContestsSpecification Instance { get; } = new AllContestsSpecification();

        public Expression<Func<Contest, bool>> IsSatisfiedBy { get; } = a => true;
    }
}
