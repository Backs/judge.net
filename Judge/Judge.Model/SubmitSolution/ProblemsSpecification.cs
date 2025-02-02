using System;
using System.Linq.Expressions;

namespace Judge.Model.SubmitSolution
{
    public sealed class ProblemsSpecification : ISpecification<SubmitResult>
    {
        private ProblemsSpecification()
        {
            
        }
        public static ISpecification<SubmitResult> Instance { get; } = new ProblemsSpecification();
        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => t => t.Submit is ProblemSubmit;
    }
}
