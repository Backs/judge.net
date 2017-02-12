using System;
using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Problems.ProblemsList;
using Judge.Application.ViewModels.Problems.Statement;
using Judge.Data;
using Judge.Model.CheckSolution;

namespace Judge.Application
{
    internal sealed class ProblemsService : IProblemsService
    {
        private readonly IUnitOfWorkFactory _factory;

        public ProblemsService(IUnitOfWorkFactory factory)
        {
            _factory = factory;
        }

        public ProblemsListViewModel GetProblemsList(int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            using (var unitOfWork = _factory.GetUnitOfWork(transactionRequired: false))
            {
                var taskRepository = unitOfWork.GetRepository<ITaskRepository>();
                var tasks = taskRepository.GetTaskList(page, pageSize)
                    .Select(o => new ProblemItem
                    {
                        Id = o.Id,
                        Name = o.Name
                    });

                return new ProblemsListViewModel(tasks);

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
