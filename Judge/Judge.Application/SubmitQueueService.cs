using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.SubmitSolution;

namespace Judge.Application
{
    internal sealed class SubmitQueueService : ISubmitQueueService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public SubmitQueueService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public SubmitQueueViewModel GetSubmitQueue(long userId, long problemId)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(false))
            {
                var submitResultRepository = uow.GetRepository<ISubmitResultRepository>();
                var languageRepository = uow.GetRepository<ILanguageRepository>();
                var taskRepository = uow.GetRepository<ITaskNameRepository>();
                var userRepository = uow.GetRepository<IUserRepository>();

                var languages = languageRepository.GetLanguages().ToDictionary(o => o.Id, o => o.Name);
                var submits = submitResultRepository.GetLastSubmits(userId, problemId, 10).ToArray();

                var task = taskRepository.GetTasks(new[] { problemId }).First();
                var user = userRepository.GetUsers(new[] { userId }).First();

                var items = submits.Select(o => new SubmitQueueItem(o, languages[o.Submit.LanguageId], task.Name, user.UserName));

                var model = new SubmitQueueViewModel(items);

                return model;
            }
        }
    }
}
