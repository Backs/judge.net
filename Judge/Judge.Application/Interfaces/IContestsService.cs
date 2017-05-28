using System.Web;
using Judge.Application.ViewModels.Contests;
using Judge.Application.ViewModels.Contests.ContestsList;
using Judge.Application.ViewModels.Contests.ContestTasks;

namespace Judge.Application.Interfaces
{
    public interface IContestsService
    {
        ContestsListViewModel GetContests();
        ContestTasksViewModel GetTasks(int contestId);
        ContestStatementViewModel GetStatement(int contestId, string label);
        void SubmitSolution(int contestId, string label, int selectedLanguage, HttpPostedFileBase file, long userId);
    }
}
