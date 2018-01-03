using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.User;
using Judge.Data;
using Judge.Model.Account;
using Judge.Model.CheckSolution;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;

namespace Judge.Application
{
    internal sealed class UserService : IUserService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public UserViewModel GetUserInfo(long id)
        {
            using (var uow = _unitOfWorkFactory.GetUnitOfWork(false))
            {
                var userRepository = uow.GetRepository<IUserRepository>();

                var user = userRepository.GetUser(id);

                if (user == null)
                {
                    return null;
                }

                var submitRepository = uow.GetRepository<ISubmitRepository>();
                var taskNameRepository = uow.GetRepository<ITaskNameRepository>();

                var solvedTasks = submitRepository.Get(new UserSolvedSpecification(id)).Select(o => o.ProblemId).ToArray();
                var unsolvedTasks = submitRepository.Get(new UserUnsolvedSpecification(id)).Select(o => o.ProblemId).ToArray();

                var tasks = taskNameRepository.GetTasks(solvedTasks.Concat(unsolvedTasks)).Where(o => o.IsOpened);

                var userProblems = tasks.Select(o => new UserProblem { Id = o.Id, Name = o.Name, Solved = solvedTasks.Contains(o.Id) }).ToArray();
                var result = new UserViewModel(userProblems)
                {
                    Id = id,
                    UserName = user.UserName
                };
                return result;
            }
        }
    }
}
