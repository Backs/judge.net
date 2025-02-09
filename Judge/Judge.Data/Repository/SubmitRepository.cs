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

    public async Task<IReadOnlyCollection<SubmitBase>> SearchAsync(ISpecification<SubmitBase> specification)
    {
        return await this.context.Set<SubmitBase>().AsNoTracking().Where(specification.IsSatisfiedBy).ToListAsync();
    }
}