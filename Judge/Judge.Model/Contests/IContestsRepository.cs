using System.Collections.Generic;
using System.Threading.Tasks;

namespace Judge.Model.Contests
{
    public interface IContestsRepository
    {
        Task<Contest> TryGetAsync(int id);
        void Add(Contest contest);
        Task<IReadOnlyList<Contest>> SearchAsync(ISpecification<Contest> specification, int skip, int take);
        Task<int> CountAsync(ISpecification<Contest> specification);
    }
}