namespace Judge.Model.Configuration
{
    using System.Collections.Generic;
    using Judge.Model.Entities;

    public interface ILanguageRepository
    {
        IEnumerable<Language> GetLanguages(bool activeOnly);
        Language Get(int id);
        void Add(Language language);
        void Delete(Language language);
    }
}
