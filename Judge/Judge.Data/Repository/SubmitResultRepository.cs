using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model;
using Judge.Model.SubmitSolution;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data.Repository
{
    internal sealed class SubmitResultRepository : ISubmitResultRepository
    {
        private readonly DataContext context;

        public SubmitResultRepository(DataContext context)
        {
            this.context = context;
        }

        public SubmitResult? Get(long id)
        {
            return this.context.Set<SubmitResult>().Where(o => o.Id == id).Include(o => o.Submit).FirstOrDefault();
        }

        public IEnumerable<SubmitResult> GetSubmits(ISpecification<SubmitResult> specification, int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            var query = this.context.Set<SubmitResult>() as IQueryable<SubmitResult>;

            query = query.Where(specification.IsSatisfiedBy);

            query = query.OrderByDescending(o => o.Id);

            var skip = (page - 1) * pageSize;

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            query = query.Take(pageSize);

            return query.Include(o => o.Submit).AsEnumerable();
        }

        public IEnumerable<long> GetSolvedProblems(ISpecification<SubmitResult> specification)
        {
            return this.context.Set<SubmitResult>()
                .Where(specification.IsSatisfiedBy)
                .Select(o => o.Submit.ProblemId)
                .Distinct()
                .AsEnumerable();
        }

        public Task<long[]> GetSolvedProblemsAsync(ISpecification<SubmitResult> specification)
        {
            return this.context.Set<SubmitResult>()
                .Where(specification.IsSatisfiedBy)
                .Select(o => o.Submit.ProblemId)
                .Distinct()
                .ToArrayAsync();
        }

        public SubmitResult? DequeueUnchecked()
        {
            var check = this.context.DequeueSubmitCheck();

            if (check == null) return null;

            return this.context.Set<SubmitResult>().Where(o => o.Id == check.SubmitResultId)
                .Include(o => o.Submit).First();
        }

        public int Count(ISpecification<SubmitResult> specification)
        {
            var query = this.context.Set<SubmitResult>()
                .Where(specification.IsSatisfiedBy);

            return query.Count();
        }
    }
}