using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Judge.Model.SubmitSolution;

namespace Judge.Model.Contests
{
    public sealed class UserContestSolvedProblemsSpecification : ISpecification<SubmitResult>
    {
        public UserContestSolvedProblemsSpecification(int contestId, long userId, IEnumerable<long> problems)
        {
            ContestId = contestId;
            UserId = userId;
            Problems = problems.Distinct();
        }

        public IEnumerable<long> Problems { get; }
        public long UserId { get; }
        public int ContestId { get; }
        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => o => o.Submit is ContestTaskSubmit &&
                                                                          (o.Submit as ContestTaskSubmit).ContestId == ContestId &&
                                                                          o.Submit.UserId == UserId &&
                                                                          Problems.Contains(o.Submit.ProblemId);
    }
}
