using System;
using System.Collections.Generic;
using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Problems.ProblemsList;
using Judge.Application.ViewModels.Problems.Statement;
using Judge.Data;
using Judge.Model;
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

        public ProblemsListViewModel GetProblemsList(int page, int pageSize, long? userId, bool openedOnly)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var taskRepository = unitOfWork.TaskNameRepository;
                var submitResultRepository = unitOfWork.SubmitResultRepository;

                var tasks = GetProblems(page, pageSize, taskRepository, openedOnly);

                var count = taskRepository.Count(openedOnly);

                var solvedTasks = new HashSet<long>();

                if (userId != null)
                {
                    solvedTasks.UnionWith(submitResultRepository.GetSolvedProblems(new UserSolvedProblemsSpecification(userId.Value, tasks.Select(o => o.Id))));
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

        private static ProblemItem[] GetProblems(int page, int pageSize, ITaskNameRepository taskRepository, bool openedOnly)
        {
            var specification = openedOnly ? (ISpecification<Task>)OpenedTasksSpecification.Instance : AllTasksSpecification.Instance;
            var tasks = taskRepository.GetTasks(specification, page, pageSize)
                .Select(o => new ProblemItem
                {
                    Id = o.Id,
                    Name = o.Name,
                    IsOpened = o.IsOpened
                }).ToArray();
            return tasks;
        }

        public StatementViewModel GetStatement(long id)
        {
            using (var unitOfWork = _factory.GetUnitOfWork())
            {
                var taskRepository = unitOfWork.TaskRepository;
                var task = taskRepository.Get(id);
                if (task == null)
                    return null;

                if (!task.IsOpened)
                {
                    return null;
                }

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

        public IReadOnlyCollection<ProblemItem> GetAllProblems()
        {
            using (var uow = _factory.GetUnitOfWork())
            {
                var taskRepository = uow.TaskNameRepository;
                return taskRepository.GetTasks(AllTasksSpecification.Instance, 1, int.MaxValue)
                    .Select(o => new ProblemItem
                    {
                        Id = o.Id,
                        Name = o.Name,
                        IsOpened = o.IsOpened
                    }).ToArray();
            }
        }
    }
}
