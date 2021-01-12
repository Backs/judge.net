namespace Judge.Application.Interfaces
{
    using System.Web;
    using Judge.Application.ViewModels;
    using Judge.Application.ViewModels.Contests;
    using Judge.Application.ViewModels.Contests.ContestResult;
    using Judge.Application.ViewModels.Contests.ContestsList;
    using Judge.Application.ViewModels.Contests.ContestTasks;
    using Judge.Application.ViewModels.Submit;

    public interface IContestsService
    {
        ContestsListViewModel GetContests(bool showAll);

        ContestTasksViewModel GetTasks(int contestId, long? userId);

        ContestStatementViewModel GetStatement(int contestId, string label);

        SubmitQueueViewModel GetSubmitQueue(long userId, int contestId, string label, int page, int pageSize);

        ContestResultViewModel GetResults(int id);

        void SubmitSolution(int contestId, string label, int selectedLanguage, HttpPostedFileBase file, UserInfo userInfo);
    }
}
