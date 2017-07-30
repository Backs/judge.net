using System.Collections.Generic;
using System.Web;
using Judge.Application.ViewModels.Problems.Solution;
using Judge.Application.ViewModels.Submit;

namespace Judge.Application.Interfaces
{
    public interface ISubmitSolutionService
    {
        IEnumerable<LanguageViewModel> GetLanguages();
        void SubmitSolution(long problemId, int selectedLanguage, HttpPostedFileBase file, long userId, string userHost);
        SolutionViewModel GetSolution(long submitId, long userId);
    }
}
