using System.Linq;
using System.Threading.Tasks;
using Judge.Data;
using Judge.Web.Client.Admin;

namespace Judge.Services;

internal sealed class LanguageService : ILanguageService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public LanguageService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<LanguageList> GetListAsync()
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);

        var languages = await unitOfWork.Languages.GetAllAsync(false);

        var result = new LanguageList
        {
            Items = languages.Select(o => new Language
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                CompilerPath = o.CompilerPath,
                IsCompilable = o.IsCompilable,
                CompilerOptionsTemplate = o.CompilerOptionsTemplate,
                IsHidden = o.IsHidden,
                OutputFileTemplate = o.OutputFileTemplate,
                RunStringTemplate = o.RunStringFormat,
                DefaultFileName = o.DefaultFileName,
                AutoDetectFileName = o.AutoDetectFileName
            }).ToArray()
        };

        return result;
    }

    public async Task<Language?> SaveAsync(EditLanguage editLanguage)
    {
        await using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork(false);

        Judge.Model.Entities.Language language;
        if (editLanguage.Id != null)
        {
            language = await unitOfWork.Languages.GetAsync(editLanguage.Id.Value);
            if (language == null)
            {
                return null;
            }
        }
        else
        {
            language = new Judge.Model.Entities.Language();
        }

        language.Description = editLanguage.Description;
        language.Name = editLanguage.Name;
        language.IsHidden = editLanguage.IsHidden;
        language.DefaultFileName = editLanguage.DefaultFileName;
        language.CompilerPath = editLanguage.CompilerPath;
        language.IsCompilable = editLanguage.IsCompilable;
        language.CompilerOptionsTemplate = editLanguage.CompilerOptionsTemplate;
        language.AutoDetectFileName = editLanguage.AutoDetectFileName;
        language.RunStringFormat = editLanguage.RunStringTemplate;
        language.OutputFileTemplate = editLanguage.OutputFileTemplate;

        if (editLanguage.Id == null)
            unitOfWork.Languages.Add(language);

        await unitOfWork.CommitAsync();

        return new Language
        {
            Id = language.Id,
            AutoDetectFileName = language.AutoDetectFileName,
            IsHidden = language.IsHidden,
            Name = language.Name,
            Description = language.Description,
            CompilerPath = language.CompilerPath,
            IsCompilable = language.IsCompilable,
            CompilerOptionsTemplate = language.CompilerOptionsTemplate,
            OutputFileTemplate = language.OutputFileTemplate,
            DefaultFileName = language.DefaultFileName,
            RunStringTemplate = language.RunStringFormat
        };
    }
}