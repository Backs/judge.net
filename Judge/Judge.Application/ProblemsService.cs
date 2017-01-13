using System;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Problems.ProblemsList;
using Judge.Application.ViewModels.Problems.Statement;

namespace Judge.Application
{
    internal sealed class ProblemsService : IProblemsService
    {
        public ProblemsListViewModel GetProblemsList(int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            return new ProblemsListViewModel(new[]
                {
                    new ProblemItem {Id = 1, Name = "Test1", Solved = true},
                    new ProblemItem {Id = 2, Name = "Test2", Solved = false}
                });
        }

        public StatementViewModel GetStatement(long id)
        {
            return new StatementViewModel
            {
                Id = id,
                CreationDate = DateTime.Now,
                MemoryLimitBytes = 1024 * 10 * 10,
                Name = "Test" + id,
                Statement = "**Statement**",
                TimeLimitMilliseconds = 1000
            };
        }
    }
}
