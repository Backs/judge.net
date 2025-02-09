using System.Threading.Tasks;

namespace Judge.Model.Configuration
{
    using System.Collections.Generic;
    using Judge.Model.Entities;

    public interface ILanguageRepository
    {
        Task<IReadOnlyCollection<Language>> GetAllAsync(bool activeOnly);
        Language Get(int id);
    }
}
