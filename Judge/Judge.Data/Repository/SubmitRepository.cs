using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data.Repository;

internal sealed class SubmitRepository : ISubmitRepository
{
    private readonly DataContext context;

    public SubmitRepository(DataContext context)
    {
        this.context = context;
    }

    public void Add(SubmitBase item)
    {
        this.context.Set<SubmitBase>().Add(item);
    }

    public SubmitBase? Get(long submitId)
    {
        return this.context.Set<SubmitBase>().AsNoTracking().FirstOrDefault(o => o.Id == submitId);
    }

    public IEnumerable<SubmitBase> Get(ISpecification<SubmitBase> specification)
    {
        return this.context.Set<SubmitBase>().AsNoTracking().Where(specification.IsSatisfiedBy).AsEnumerable();
    }

    public async Task<IReadOnlyCollection<SubmitBase>> SearchAsync(ISpecification<SubmitBase> specification)
    {
        return await this.context.Set<SubmitBase>().AsNoTracking().Where(specification.IsSatisfiedBy).ToListAsync();
    }

    public Task<SubmitBase?> GetAsync(long id)
    {
        return this.context.Set<SubmitBase>().AsNoTracking().FirstOrDefaultAsync(o => o.Id == id)!;
    }
}