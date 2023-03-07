using System;
using System.Linq;
using System.Linq.Expressions;
using Judge.Model.SubmitSolution;

namespace Judge.Model.Entities
{
    public sealed class UserUnsolvedSpecification : ISpecification<SubmitBase>
    {
        public UserUnsolvedSpecification(long userId)
        {
            IsSatisfiedBy = submit => submit.UserId == userId &&
                                      submit is ProblemSubmit &&
                                      submit.Results.Any(o => o.Status == SubmitStatus.MemoryLimitExceeded ||
                                                              o.Status == SubmitStatus.RuntimeError ||
                                                              o.Status == SubmitStatus.TimeLimitExceeded ||
                                                              o.Status == SubmitStatus.WrongAnswer);
        }
        public Expression<Func<SubmitBase, bool>> IsSatisfiedBy { get; }
    }
}
