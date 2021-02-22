namespace Judge.Model.Entities
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Judge.Model.SubmitSolution;

    public sealed class UserSolvedSpecification : ISpecification<SubmitBase>
    {
        public UserSolvedSpecification(long userId)
        {
            this.IsSatisfiedBy = submit => submit.UserId == userId &&
                                           submit is ProblemSubmit &&
                                           submit.Results.Any(o => o.Status == SubmitStatus.Accepted);
        }

        public Expression<Func<SubmitBase, bool>> IsSatisfiedBy { get; }
    }
}
