using System;
using System.Linq.Expressions;

namespace Judge.Model.SubmitSolution
{
    public sealed class AllSubmitsSpecification : ISpecification<SubmitResult>
    {
        private AllSubmitsSpecification()
        {
            
        }

        public static ISpecification<SubmitResult> Instance { get; } = new AllSubmitsSpecification();

        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => t => true;
    }
}
