using System.Collections.Generic;
using System.Web;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;

namespace Judge.Application
{
    internal sealed class SubmitSolutionService : ISubmitSolutionService
    {
        public IEnumerable<LanguageViewModel> GetLanguages()
        {
            return new[]
            {
                new LanguageViewModel { Id = 1, Name = "C++" },
                new LanguageViewModel { Id = 2, Name = "C#" }
            };
        }

        public void SubmitSolution(long problemId, long selectedLanguage, HttpPostedFileBase file)
        {

        }
    }
}
