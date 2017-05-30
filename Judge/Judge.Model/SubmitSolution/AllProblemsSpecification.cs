using System;
using System.Linq.Expressions;

namespace Judge.Model.SubmitSolution
{
    public sealed class AllProblemsSpecification : ISpecification<SubmitResult>
    {
        private AllProblemsSpecification()
        {
            
        }
        public static ISpecification<SubmitResult> Instance { get; } = new AllProblemsSpecification();
        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => t => t.Submit is ProblemSubmit;
    }
}
