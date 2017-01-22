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
    }
}
