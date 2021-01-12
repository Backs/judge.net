namespace Judge.Application.Interfaces
{
    using System.Collections.Generic;
    using Judge.Application.ViewModels.Admin.Contests;
    using Judge.Application.ViewModels.Admin.Languages;
    using Judge.Application.ViewModels.Admin.Problems;
    using Judge.Application.ViewModels.Admin.Submits;
    using Judge.Application.ViewModels.Admin.Users;

    public interface IAdminService
    {
        List<LanguageEditViewModel> GetLanguages();

        void SaveLanguages(ICollection<LanguageEditViewModel> languages);

        IEnumerable<SubmitQueueItem> GetSubmitQueue();

        EditProblemViewModel GetProblem(long id);

        long SaveProblem(EditProblemViewModel model);

        EditContestViewModel GetContest(int id);

        int SaveContest(EditContestViewModel model);

        UserListViewModel GetUsers();

        UserEditViewModel GetUser(long id);
    }
}
