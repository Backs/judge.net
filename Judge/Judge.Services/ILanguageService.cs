using System.Threading.Tasks;
using Judge.Web.Client.Admin;

namespace Judge.Services;

public interface ILanguageService
{
    Task<LanguageList> GetListAsync();

    Task<Language?> SaveAsync(EditLanguage editLanguage);
}