using System.Collections.Generic;
using Judge.Application.ViewModels.Admin.Languages;

namespace Judge.Application.Interfaces
{
    public interface IAdminService
    {
        List<LanguageEditViewModel> GetLanguages();
        void SaveLanguages(ICollection<LanguageEditViewModel> languages);
    }
}
