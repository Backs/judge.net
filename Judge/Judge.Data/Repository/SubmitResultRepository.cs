using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data.Repository;

internal sealed class SubmitResultRepository : ISubmitResultRepository
{
    private readonly DataContext context;

    public SubmitResultRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyCollection<SubmitResult>> SearchAsync(ISpecification<SubmitResult> specification,
        int skip, int take)
    {
        IQueryable<SubmitResult> query = this.context.Set<SubmitResult>();

        query = query.Where(specification.IsSatisfiedBy);

        query = query.OrderByDescending(o => o.Id);

        if (skip > 0)
        {
            query = query.Skip(skip);
        }

        query = query.Take(take);

        return await query.Include(o => o.Submit).ToListAsync();
    }

    public async Task<IReadOnlyCollection<long>> GetSolvedProblemsAsync(ISpecification<SubmitResult> specification)
    {
        return await this.context.Set<SubmitResult>()
            .Where(specification.IsSatisfiedBy)
            .Select(o => o.Submit.ProblemId)
            .Distinct()
            .ToListAsync();
    }

    public SubmitResult? DequeueUnchecked()
    {
        var check = this.context.DequeueSubmitCheck();

        if (check == null) return null;

        return this.context.Set<SubmitResult>().Where(o => o.Id == check.SubmitResultId)
            .Include(o => o.Submit)
            .Include(o => o.Submit.User)
            .First();
    }

    public Task<int> CountAsync(ISpecification<SubmitResult> specification)
    {
        return this.context.Set<SubmitResult>()
            .Where(specification.IsSatisfiedBy).CountAsync();
    }

    public Task<SubmitResult?> GetAsync(long id)
    {
        return this.context.Set<SubmitResult>().Where(o => o.Id == id)
            .Include(o => o.Submit).FirstOrDefaultAsync();
    }

    public async Task<SubmitResult> SaveAsync(SubmitResult submitResult)
    {
        var result = await this.context.Set<SubmitResult>().AddAsync(submitResult);
        return result.Entity;
    }
}