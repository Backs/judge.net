using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model.Entities;

namespace Judge.Model.Configuration;

public static class ILanguageRepositoryExtensions
{
    public static async Task<IReadOnlyDictionary<int, Language>> GetDictionaryAsync(
        this ILanguageRepository repository, bool activeOnly)
    {
        var result = await repository.GetAllAsync(activeOnly);
        return result.ToDictionary(o => o.Id);
    }
}