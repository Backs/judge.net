using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public interface ISubmitResultRepository
    {
        IEnumerable<SubmitResult> GetLastSubmits(long? userId, long? problemId, int count);

        IEnumerable<long> GetSolvedProblems(IEnumerable<long> problems);

        SubmitResult DequeueUnchecked();
    }
}
