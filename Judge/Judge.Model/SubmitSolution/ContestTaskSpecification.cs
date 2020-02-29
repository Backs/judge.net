using System;
using System.Linq.Expressions;

namespace Judge.Model.SubmitSolution
{
    public sealed class ContestTaskSpecification : ISpecification<SubmitResult>
    {
        private readonly int _contestId;
        private readonly long _problemId;
        private readonly long _userId;

        public ContestTaskSpecification(int contestId, long problemId, long userId)
        {
            _contestId = contestId;
            _userId = userId;
            _problemId = problemId;
        }

        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy => t =>
                    t.Submit is ContestTaskSubmit &&
                    ((ContestTaskSubmit)t.Submit).ContestId == _contestId &&
                    t.Submit.ProblemId == _problemId &&
                    t.Submit.UserId == _userId;
    }
}
