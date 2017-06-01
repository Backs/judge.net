using System.Collections.Generic;

namespace Judge.Model.Contests
{
    public interface IContestResultRepository
    {
        IEnumerable<ContestResult> Get(long contestId);
    }
}
