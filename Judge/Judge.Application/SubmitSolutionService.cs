using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
using Judge.Model.Configuration;
using Judge.Model.SubmitSolution;

namespace Judge.Application
{
    internal sealed class SubmitSolutionService : ISubmitSolutionService
    {
        private readonly IUnitOfWorkFactory _factory;

        public SubmitSolutionService(IUnitOfWorkFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<LanguageViewModel> GetLanguages()
        {
            using (var uow = _factory.GetUnitOfWork(false))
            {
                var languageRepository = uow.GetRepository<ILanguageRepository>();

                return languageRepository.GetLanguages().Select(o => new LanguageViewModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).AsEnumerable();
            }
        }

        public void SubmitSolution(long problemId, int selectedLanguage, HttpPostedFileBase file, long userId)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string sourceCode;
            using (var sr = new StreamReader(file.InputStream))
            {
                sourceCode = sr.ReadToEnd();
            }

            var submit = Submit.Create();

            submit.ProblemId = problemId;
            submit.LanguageId = selectedLanguage;
            submit.UserId = userId;
            submit.FileName = file.FileName;
            submit.SourceCode = sourceCode;

            using (var unitOfWork = _factory.GetUnitOfWork(true))
            {
                var submitRepository = unitOfWork.GetRepository<ISubmitRepository>();
                submitRepository.Add(submit);
                unitOfWork.Commit();
            }
        }
    }
}
