using System.Collections.Generic;

namespace Judge.Model.SubmitSolution
{
    public interface ISubmitResultRepository
    {
        IEnumerable<SubmitResult> GetLastSubmits(long? userId, int count);
    }
}
