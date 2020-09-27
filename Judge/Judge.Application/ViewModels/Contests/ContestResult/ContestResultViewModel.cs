using System.Collections.Generic;
using System.Linq;
using Judge.Application.ViewModels.Contests.ContestsList;

namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public class ContestResultViewModel
    {
        public ContestResultViewModel(IEnumerable<ContestUserResultViewModelBase> users)
        {
            var result = new List<ContestUserResultViewModelBase>();

            var place = 1;
            foreach (var user in users.OrderByDescending(o => o))
            {
                if (result.Count != 0 && result.Last().CompareTo(user) != 0)
                {
                    place++;
                }

                user.Place = place;
                result.Add(user);
            }

            Users = result;
        }

        public ContestItem Contest { get; set; }

        public IEnumerable<TaskViewModel> Tasks { get; set; }

        public IEnumerable<ContestUserResultViewModelBase> Users { get; }
    }
}