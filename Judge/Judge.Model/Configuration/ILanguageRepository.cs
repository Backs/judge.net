using System.Collections.Generic;
using Judge.Model.Entities;

namespace Judge.Model.Configuration
{
    public interface ILanguageRepository
    {
        IEnumerable<Language> GetLanguages();
    }
}
