using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Judge.Model.CheckSolution
{
    public static class ITaskRepositoryExtensions
    {
        public static async Task<IReadOnlyDictionary<long, Task>> GetDictionaryAsync(this ITaskRepository repository,
            IEnumerable<long> ids)
        {
            var result = await repository.GetAsync(ids);
            return result.ToDictionary(o => o.Id);
        }
    }
}