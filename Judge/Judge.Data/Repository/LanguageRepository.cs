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
            return _context.Set<Language>().OrderBy(o => o.Id).AsEnumerable();
        }
    }
}
