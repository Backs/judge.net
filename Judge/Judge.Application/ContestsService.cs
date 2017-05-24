using System.Linq;
using Judge.Application.Interfaces;
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

                var items = repository.GetList().Select(o => new ContestItem
                {
                    Id = o.Id,
                    Name = o.Name,
                    StartTime = o.StartTime,
                    Duration = o.FinishTime - o.StartTime
                });
                return new ContestsListViewModel(items);
            }
        }

        public ContestTasksViewModel GetTasks(long contestId)
        {
            using (var unitOfWork = _factory.GetUnitOfWork(false))
            {

            }
            return new ContestTasksViewModel();
        }
    }
}
