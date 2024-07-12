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
        Task<IReadOnlyList<Contest>> SearchAsync(ISpecification<Contest> specification, int skip, int take);
        Task<int> CountAsync(ISpecification<Contest> specification);
    }
}