using System.Collections.Generic;
using System.Web;
using Judge.Application.ViewModels.Submit;

namespace Judge.Application.Interfaces
{
    public interface ISubmitSolutionService
    {
        IEnumerable<LanguageViewModel> GetLanguages();
        void SubmitSolution(long problemId, long selectedLanguage, HttpPostedFileBase file);
    }
}
