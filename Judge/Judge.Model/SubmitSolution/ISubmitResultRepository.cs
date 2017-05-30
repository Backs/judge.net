using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public interface ISubmitResultRepository
    {
        IEnumerable<SubmitResult> GetSubmits(ISpecification<SubmitResult> specification, int page, int pageSize);

        IEnumerable<long> GetSolvedProblems(long userId, IEnumerable<long> problems);

        SubmitResult DequeueUnchecked();
        int Count(ISpecification<SubmitResult> specification);
    }
}
