using System;
using System.Collections.Generic;
using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Problems.ProblemsList;
using Judge.Application.ViewModels.Problems.Statement;
using Judge.Data;
using Judge.Model.CheckSolution;
using Judge.Model.SubmitSolution;

namespace Judge.Application
{
    internal sealed class ProblemsService : IProblemsService
    {
        private readonly IUnitOfWorkFactory _factory;

        public ProblemsService(IUnitOfWorkFactory factory)
        {
            _factory = factory;
        }

        public ProblemsListViewModel GetProblemsList(int page, int pageSize, long? userId)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            using (var unitOfWork = _factory.GetUnitOfWork(transactionRequired: false))
            {
                var taskRepository = unitOfWork.GetRepository<ITaskNameRepository>();
                var submitResultRepository = unitOfWork.GetRepository<ISubmitResultRepository>();

                var tasks = taskRepository.GetTasks(page, pageSize)
                    .Select(o => new ProblemItem
                    {
                        Id = o.Id,
                        Name = o.Name
                    }).ToArray();

                var count = taskRepository.Count();

                var solvedTasks = new HashSet<long>();

                if (userId != null)
                {
                    solvedTasks.UnionWith(submitResultRepository.GetSolvedProblems(userId.Value, tasks.Select(o => o.Id)));
                }

                foreach (var item in tasks)
                {
                    item.Solved = solvedTasks.Contains(item.Id);
                }

                return new ProblemsListViewModel(tasks)
                {
                    ProblemsCount = count,
                    Pagination = new ViewModels.PaginationViewModel { CurrentPage = page, PageSize = pageSize, TotalPages = (count + pageSize - 1) / pageSize }
                };

            }
        }

        public StatementViewModel GetStatement(long id)
        {
            using (var unitOfWork = _factory.GetUnitOfWork(transactionRequired: false))
            {
                var taskRepository = unitOfWork.GetRepository<ITaskRepository>();
                var task = taskRepository.Get(id);

                return new StatementViewModel
                {
                    Id = task.Id,
                    CreationDate = task.CreationDateUtc,
                    MemoryLimitBytes = task.MemoryLimitBytes,
                    Name = task.Name,
                    Statement = task.Statement,
                    TimeLimitMilliseconds = task.TimeLimitMilliseconds
                };
            }
        }
    }
}
