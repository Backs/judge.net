namespace Judge.Model.SubmitSolution
{
    using System;
    using System.Linq.Expressions;

    public sealed class SubmitsSpecification : ISpecification<SubmitResult>
    {
        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy { get; } = t => true;

        public SubmitsSpecification(
            SubmitsType type = SubmitsType.All,
            int? language = null,
            SubmitStatus? status = null,
            long? problemId = null,
            int? contestId = null,
            long? userId = null)
        {
            this.IsSatisfiedBy = type switch
            {
                SubmitsType.Contest => this.IsSatisfiedBy.And(t => t.Submit is ContestTaskSubmit),
                SubmitsType.Problem => this.IsSatisfiedBy.And(t => t.Submit is ProblemSubmit),
                _ => this.IsSatisfiedBy
            };

            if (language != null)
            {
                this.IsSatisfiedBy = this.IsSatisfiedBy.And(t => t.Submit.LanguageId == language);
            }

            if (status != null)
            {
                this.IsSatisfiedBy = this.IsSatisfiedBy.And(t => t.Status == status);
            }

            if (problemId != null)
            {
                this.IsSatisfiedBy = this.IsSatisfiedBy.And(t => t.Submit.ProblemId == problemId);
            }

            if (userId != null)
            {
                this.IsSatisfiedBy = this.IsSatisfiedBy.And(t => t.Submit.UserId == userId);
            }

            if (contestId != null)
            {
                this.IsSatisfiedBy = this.IsSatisfiedBy.And(t =>
                    t.Submit is ContestTaskSubmit && ((ContestTaskSubmit) t.Submit).ContestId == contestId);
            }
        }
    }
}