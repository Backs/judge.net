using System.Collections.Generic;
using System.Linq;
using Judge.Model.Configuration;
using Judge.Model.Entities;

namespace Judge.Data.Repository
{
    internal sealed class LanguageRepository : ILanguageRepository
    {
        private readonly DataContext _context;

        public LanguageRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Language> GetLanguages()
        {
            return _context.Set<Language>().AsNoTracking().OrderBy(o => o.Id).AsEnumerable();
        }

        public Language Get(int id)
        {
            return _context.Set<Language>().AsNoTracking().FirstOrDefault(o => o.Id == id);
        }

        public void Save(Language language)
        {
            _context.Set<Language>().Attach(language);
            if (language.Id == 0)
                _context.Entry(language).State = System.Data.Entity.EntityState.Added;
            else
                _context.Entry(language).State = System.Data.Entity.EntityState.Modified;

        }

        public void Delete(Language language)
        {
            _context.Set<Language>().Attach(language);
            _context.Entry(language).State = System.Data.Entity.EntityState.Deleted;
        }
    }
}
