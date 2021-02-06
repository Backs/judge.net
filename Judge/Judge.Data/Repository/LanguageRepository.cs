namespace Judge.Data.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using Judge.Model.Configuration;
    using Judge.Model.Entities;

    internal sealed class LanguageRepository : ILanguageRepository
    {
        private readonly DataContext context;

        public LanguageRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<Language> GetLanguages(bool activeOnly)
        {
            IQueryable<Language> query = this.context.Set<Language>();

            if (activeOnly)
            {
                query = query.Where(o => o.IsHidden == false);
            }

            return query.OrderBy(o => o.Name).AsEnumerable();
        }

        public Language Get(int id)
        {
            return this.context.Set<Language>().FirstOrDefault(o => o.Id == id);
        }

        public void Add(Language language)
        {
            this.context.Set<Language>().Add(language);
        }

        public void Delete(Language language)
        {
            this.context.Set<Language>().Remove(language);
        }
    }
}
