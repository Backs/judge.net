using System.Threading.Tasks;
using System.Collections.Generic;
using Judge.Model.Entities;

namespace Judge.Model.Configuration;

public interface ILanguageRepository
{
    Task<IReadOnlyCollection<Language>> GetAllAsync(bool activeOnly);
    Task<Language> GetAsync(int id);
    Language Get(int id);
    void Add(Language language);
}