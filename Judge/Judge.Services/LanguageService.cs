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
                RunStringTemplate = o.RunStringFormat
            }).ToArray()
        };

        return result;
    }
}