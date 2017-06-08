using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Judge.Model.SubmitSolution
{
    public sealed class UserSolvedProblemsSpecification : ISpecification<SubmitResult>
    {
        public UserSolvedProblemsSpecification(long userId, IEnumerable<long> problems)
        {
            Problems = problems.Distinct();
            UserId = userId;
        }

        public IEnumerable<long> Problems { get; }
        public long UserId { get; }

        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => o => o.Submit is ProblemSubmit && o.Submit.UserId == UserId &&
                                                                          Problems.Contains(o.Submit.ProblemId);
    }
}
