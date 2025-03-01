using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model.Entities;

namespace Judge.Model.Account;

public static class IUserRepositoryExtensions
{
    public static async Task<IReadOnlyDictionary<long, User>> GetDictionaryAsync(this IUserRepository repository,
        ISpecification<User> specification)
    {
        var result = await repository.SearchAsync(specification);
        return result.ToDictionary(o => o.Id);
    }
}