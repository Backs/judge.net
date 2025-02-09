using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model.Configuration;
using Judge.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data.Repository;

internal sealed class LanguageRepository : ILanguageRepository
{
    private readonly DataContext context;

    public LanguageRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyCollection<Language>> GetAllAsync(bool activeOnly)
    {
        IQueryable<Language> query = this.context.Set<Language>();

        if (activeOnly)
        {
            query = query.Where(o => o.IsHidden == false);
        }

        return await query.OrderBy(o => o.Name).ToListAsync();
    }

    public Language? Get(int id)
    {
        return this.context.Set<Language>().FirstOrDefault(o => o.Id == id);
    }
}