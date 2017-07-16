using System.Collections.Generic;
using Judge.Application.ViewModels.Admin.Languages;
using Judge.Application.ViewModels.Admin.Submits;

namespace Judge.Application.Interfaces
{
    public interface IAdminService
    {
        List<LanguageEditViewModel> GetLanguages();
        void SaveLanguages(ICollection<LanguageEditViewModel> languages);
        IEnumerable<SubmitQueueItem> GetSubmitQueue();
    }
}
