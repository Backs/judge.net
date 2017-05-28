using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Judge.Application.ViewModels.Submit;

namespace Judge.Application.ViewModels.Contests
{
    public class SubmitContestSolutionViewModel
    {
        public IEnumerable<LanguageViewModel> Languages { get; set; }
        public int SelectedLanguage { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "SelectFile")]
        [FileValidation]
        [MaxFileSize(100 * 1024)]
        public HttpPostedFileBase File { get; set; }

        public bool Success { get; set; }
        public string Label { get; set; }
        public int ContestId { get; set; }
    }
}