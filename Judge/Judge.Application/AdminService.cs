using System.Collections.Generic;
using System.Linq;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Admin.Languages;
using Judge.Data;
using Judge.Model.Configuration;
using Judge.Model.Entities;

namespace Judge.Application
{
    internal sealed class AdminService : IAdminService
    {
        private readonly IUnitOfWorkFactory _factory;

        public AdminService(IUnitOfWorkFactory factory)
        {
            _factory = factory;
        }

        public List<LanguageEditViewModel> GetLanguages()
        {
            using (var uow = _factory.GetUnitOfWork(false))
            {
                var repository = uow.GetRepository<ILanguageRepository>();
                return repository.GetLanguages().Select(o => new LanguageEditViewModel
                {
                    Id = o.Id,
                    CompilerOptionsTemplate = o.CompilerOptionsTemplate,
                    CompilerPath = o.CompilerPath,
                    Description = o.Description,
                    IsCompilable = o.IsCompilable,
                    IsHidden = o.IsHidden,
                    Name = o.Name,
                    OutputFileTemplate = o.OutputFileTemplate,
                    RunStringFormat = o.RunStringFormat
                })
                .ToList();
            }
        }

        public void SaveLanguages(ICollection<LanguageEditViewModel> languages)
        {
            using (var uow = _factory.GetUnitOfWork(true))
            {
                var repository = uow.GetRepository<ILanguageRepository>();
                var databaseLanguages = repository.GetLanguages();

                foreach (var databaseLanguage in databaseLanguages)
                {
                    var language = languages.FirstOrDefault(o => o.Id == databaseLanguage.Id);
                    if (language == null)
                    {
                        repository.Delete(databaseLanguage);
                        continue;
                    }

                    databaseLanguage.CompilerOptionsTemplate = language.CompilerOptionsTemplate;
                    databaseLanguage.CompilerPath = language.CompilerPath;
                    databaseLanguage.Description = language.Description;
                    databaseLanguage.IsCompilable = language.IsCompilable;
                    databaseLanguage.IsHidden = language.IsHidden;
                    databaseLanguage.Name = language.Name;
                    databaseLanguage.OutputFileTemplate = language.Name;
                    databaseLanguage.RunStringFormat = language.RunStringFormat;

                    repository.Save(databaseLanguage);
                }

                foreach (var language in languages.Where(o => o.Id == 0))
                {
                    var databaseLanguage = new Language
                    {
                        CompilerOptionsTemplate = language.CompilerOptionsTemplate,
                        CompilerPath = language.CompilerPath,
                        Description = language.Description,
                        IsCompilable = language.IsCompilable,
                        IsHidden = language.IsHidden,
                        Name = language.Name,
                        OutputFileTemplate = language.Name,
                        RunStringFormat = language.RunStringFormat
                    };

                    repository.Save(databaseLanguage);
                }
                uow.Commit();
            }
        }
    }
}
