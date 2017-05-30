using System;
using System.Linq.Expressions;

namespace Judge.Model.SubmitSolution
{
    public sealed class UserProblemSpecification : ISpecification<SubmitResult>
    {
        private readonly long _userId;
        private readonly long _problemId;
        public UserProblemSpecification(long userId, long problemId)
        {
            _userId = userId;
            _problemId = problemId;
        }

        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => t => t.Submit is ProblemSubmit &&
                                                                          t.Submit.UserId == _userId &&
                                                                          t.Submit.ProblemId == _problemId;
    }
}
