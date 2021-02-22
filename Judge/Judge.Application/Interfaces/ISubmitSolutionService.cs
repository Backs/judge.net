namespace Judge.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Web;
    using Judge.Application.ViewModels;
    using Judge.Application.ViewModels.Problems.Solution;
    using Judge.Application.ViewModels.Submit;

    public interface ISubmitSolutionService
    {
        IEnumerable<LanguageViewModel> GetLanguages();

        IEnumerable<LanguageViewModel> GetLanguages(int contestId, string label, long userId);

        void SubmitSolution(long problemId, int selectedLanguage, HttpPostedFileBase file, UserInfo userInfo);

        SolutionViewModel GetSolution(long submitResultId, long userId);
    }
}
