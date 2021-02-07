namespace Judge.Model.SubmitSolution
{
    using System;
    using System.Linq.Expressions;

    public sealed class AllSubmitsSpecification : ISpecification<SubmitResult>
    {
        private AllSubmitsSpecification()
        {
            
        }

        public static ISpecification<SubmitResult> Instance { get; } = new AllSubmitsSpecification();

        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => t => true;
    }
}
