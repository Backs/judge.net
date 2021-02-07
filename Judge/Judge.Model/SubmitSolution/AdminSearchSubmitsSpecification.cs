namespace Judge.Model.SubmitSolution
{
    using System;
    using System.Linq.Expressions;

    public sealed class AdminSearchSubmitsSpecification : ISpecification<SubmitResult>
    {
        public Expression<Func<SubmitResult, bool>> IsSatisfiedBy { get; } = t => true;

        public AdminSearchSubmitsSpecification(int? language, SubmitStatus? status)
        {
            if (language != null)
            {
                this.IsSatisfiedBy = t => t.Submit.LanguageId == language;
            }

            if (status != null)
            {
                this.IsSatisfiedBy = this.IsSatisfiedBy.And(t => t.Status == status);
            }
        }
    }
}
