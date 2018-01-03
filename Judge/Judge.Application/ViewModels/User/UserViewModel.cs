using System.Collections.Generic;
using System.Linq;

namespace Judge.Application.ViewModels.User
{
    public sealed class UserViewModel
    {
        public UserViewModel(ICollection<UserProblem> problems)
        {
            Problems = problems;
            Total = problems.Count;
            Solved = problems.Count(o => o.Solved);
        }

        public long Id { get; set; }

        public string UserName { get; set; }
        public int Total { get; }
        public int Solved { get; }
        public ICollection<UserProblem> Problems { get; }
    }
}
