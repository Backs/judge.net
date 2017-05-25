using System.Collections.Generic;

namespace Judge.Model.Contests
{
    public interface IContestsRepository
    {
        IEnumerable<Contest> GetList();
        Contest Get(int id);
    }
}
