using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class SubmitSolutionViewModel
    {
        public IEnumerable<LanguageViewModel> Languages { get; set; }
        public long SelectedLanguage { get; set; }

        [Required(ErrorMessage = "Выберите файл")]
        public HttpPostedFileBase File { get; set; }

        public bool Success { get; set; }
        public long ProblemId { get; set; }
    }
}
