using System.Collections.Generic;
using System.Threading.Tasks;

namespace Judge.Model.Contests
{
    public interface IContestsRepository
    {
        IEnumerable<Contest> GetList(ISpecification<Contest> specification);
        Contest Get(int id);
        Task<Contest> TryGetAsync(int id);
        void Add(Contest contest);
    }
}
