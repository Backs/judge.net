using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public interface ISubmitResultRepository
    {
        IEnumerable<SubmitResult> GetSubmits(long? userId, long? problemId, int page, int pageSize);

        IEnumerable<long> GetSolvedProblems(long userId, IEnumerable<long> problems);

        SubmitResult DequeueUnchecked();
        int Count(long? problemId);
    }
}
