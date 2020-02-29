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
            Problems = problems.Distinct().ToArray();
        }

        public long[] Problems { get; }
        public long UserId { get; }
        public int ContestId { get; }
        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => o => o.Submit is ContestTaskSubmit &&
                                                                          ((ContestTaskSubmit)o.Submit).ContestId == ContestId &&
                                                                          o.Submit.UserId == UserId &&
                                                                          Problems.Contains(o.Submit.ProblemId);
    }
}
