using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Judge.Model.SubmitSolution
{
    public sealed class UserSolvedProblemsSpecification : ISpecification<SubmitResult>
    {
        public UserSolvedProblemsSpecification(long userId, IReadOnlyCollection<long> problems)
        {
            this.Problems = problems;
            this.UserId = userId;
        }

        public IReadOnlyCollection<long> Problems { get; }
        public long UserId { get; }

        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => o =>
            o.Status == SubmitStatus.Accepted &&
            o.Submit is ProblemSubmit && o.Submit.UserId == this.UserId && this.Problems.Contains(o.Submit.ProblemId);
    }
}