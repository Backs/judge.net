using System;
using System.Linq;
using System.Linq.Expressions;
using Judge.Model.SubmitSolution;

namespace Judge.Model.Entities
{
    public sealed class UserSolvedSpecification : ISpecification<SubmitBase>
    {
        public UserSolvedSpecification(long userId)
        {
            IsSatisfiedBy = submit => submit.UserId == userId &&
                                      submit is ProblemSubmit &&
                                      submit.Results.Any(o => o.Status == SubmitStatus.Accepted);
        }
        public Expression<Func<SubmitBase, bool>> IsSatisfiedBy { get; }
    }
}
