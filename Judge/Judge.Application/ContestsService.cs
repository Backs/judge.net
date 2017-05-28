using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Contests;
using Judge.Application.ViewModels.Contests.ContestsList;
using Judge.Application.ViewModels.Contests.ContestTasks;
using Judge.Data;
using Judge.Model.Contests;

namespace Judge.Application
{
    internal sealed class ContestsService : IContestsService
    {
        private readonly IUnitOfWorkFactory _factory;

        public ContestsService(IUnitOfWorkFactory factory)
        {
            _factory = factory;
        }

        public ContestsListViewModel GetContests()
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {
                var repository = unitOfWork.GetRepository<IContestsRepository>();

                var items = repository.GetList().Select(o => new ContestItem(o));
                return new ContestsListViewModel(items);
            }
        }

        public ContestTasksViewModel GetTasks(int contestId)
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {
                var contestRepository = unitOfWork.GetRepository<IContestsRepository>();
                var contest = contestRepository.Get(contestId);
                var taskRepository = unitOfWork.GetRepository<IContestTaskRepository>();
                var items = taskRepository.GetTasks(contestId).Select(o => new ContestTaskItem
                {
                    Label = o.TaskName,
                    Name = o.Task.Name,
                    ProblemId = o.Task.Id,
                    Solved = false //TODO: change
                })
                .OrderBy(o => o.Label);

                return new ContestTasksViewModel(items)
                {
                    ContestId = contest.Id,
                    ContestName = contest.Name
                };
            }
        }

        public ContestStatementViewModel GetStatement(int contestId, string label)
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {
                var contestTaskRepository = unitOfWork.GetRepository<IContestTaskRepository>();

                var task = contestTaskRepository.Get(contestId, label);
                if (task == null)
                    return null;

                var contestRepository = unitOfWork.GetRepository<IContestsRepository>();
                var contest = contestRepository.Get(contestId);

                return new ContestStatementViewModel
                {
                    Id = task.Task.Id,
                    CreationDate = task.Task.CreationDateUtc,
                    MemoryLimitBytes = task.Task.MemoryLimitBytes,
                    Name = task.Task.Name,
                    Statement = task.Task.Statement,
                    TimeLimitMilliseconds = task.Task.TimeLimitMilliseconds,
                    Label = task.TaskName,
                    Contest = new ContestItem(contest)
                };

            }
        }
    }
}
