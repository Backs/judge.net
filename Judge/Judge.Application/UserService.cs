namespace Judge.Application
{
    using System.Linq;
    using Judge.Application.Interfaces;
    using Judge.Application.ViewModels.User;
    using Judge.Data;
    using Judge.Model.Entities;

    internal sealed class UserService : IUserService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UserService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public UserViewModel GetUserInfo(long id)
        {
            using (var uow = this.unitOfWorkFactory.GetUnitOfWork())
            {
                var userRepository = uow.Users;

                var user = userRepository.Get(id);

                if (user == null)
                {
                    return null;
                }

                var submitRepository = uow.Submits;
                var taskNameRepository = uow.TaskNames;

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
