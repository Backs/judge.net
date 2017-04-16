using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class SubmitSolutionViewModel
    {
        public IEnumerable<LanguageViewModel> Languages { get; set; }
        public int SelectedLanguage { get; set; }

        [Required(ErrorMessage = "Выберите файл")]
        [FileValidation]
        public HttpPostedFileBase File { get; set; }

        public bool Success { get; set; }
        public long ProblemId { get; set; }
    }
}
