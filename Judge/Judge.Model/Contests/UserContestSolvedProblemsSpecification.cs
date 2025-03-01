using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Judge.Model.SubmitSolution;

namespace Judge.Model.Contests;

public sealed class UserContestSolvedProblemsSpecification : ISpecification<SubmitResult>
{
    public UserContestSolvedProblemsSpecification(int contestId, long userId, IEnumerable<long> problems)
    {
        this.contestId = contestId;
        this.userId = userId;
        this.problems = problems.Distinct().ToArray();
    }

    private readonly long[] problems;
    private readonly long userId;
    private readonly int contestId;

    public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => o => o.Submit is ContestTaskSubmit &&
                                                                      ((ContestTaskSubmit)o.Submit).ContestId ==
                                                                      this.contestId &&
                                                                      o.Submit.UserId == this.userId &&
                                                                      o.Status == SubmitStatus.Accepted &&
                                                                      this.problems.Contains(o.Submit.ProblemId);
}