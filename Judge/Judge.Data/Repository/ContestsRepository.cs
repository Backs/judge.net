using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model;
using Judge.Model.Contests;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data.Repository;

internal sealed class ContestsRepository : IContestsRepository
{
    private readonly DataContext context;

    public ContestsRepository(DataContext context)
    {
        this.context = context;
    }

    public IEnumerable<Contest> GetList(ISpecification<Contest> specification)
    {
        return this.context.Set<Contest>().Where(specification.IsSatisfiedBy).OrderByDescending(o => o.StartTime)
            .AsEnumerable();
    }

    public Contest? Get(int id)
    {
        return this.context.Set<Contest>().FirstOrDefault(o => o.Id == id);
    }

    public Task<Contest?> TryGetAsync(int id)
    {
        return this.context.Set<Contest>().FirstOrDefaultAsync(o => o.Id == id)!;
    }

    public void Add(Contest contest)
    {
        this.context.Set<Contest>().Add(contest);
    }

    public async Task<IReadOnlyList<Contest>> SearchAsync(ISpecification<Contest> specification, int skip, int take)
    {
        IQueryable<Contest> query = this.context.Set<Contest>()
            .Where(specification.IsSatisfiedBy)
            .OrderByDescending(o => o.StartTime);

        if (skip != 0)
        {
            query = query.Skip(skip);
        }

        query = query.Take(take);

        return await query.ToListAsync();
    }
}