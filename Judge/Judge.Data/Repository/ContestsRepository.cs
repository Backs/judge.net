﻿using System.Collections.Generic;
using System.Linq;
using Judge.Model;
using Judge.Model.Contests;

namespace Judge.Data.Repository
{
    internal sealed class ContestsRepository : IContestsRepository
    {
        private readonly DataContext context;

        public ContestsRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<Contest> GetList(ISpecification<Contest> specification)
        {
            return this.context.Set<Contest>().Where(specification.IsSatisfiedBy).OrderByDescending(o => o.StartTime).AsEnumerable();
        }

        public Contest Get(int id)
        {
            return this.context.Set<Contest>().FirstOrDefault(o => o.Id == id);
        }

        public void Add(Contest contest)
        {
            this.context.Set<Contest>().Add(contest);
        }
    }
}
