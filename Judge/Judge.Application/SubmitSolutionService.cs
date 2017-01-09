using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Judge.Application.Interfaces;
using Judge.Application.ViewModels.Submit;
using Judge.Data;
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
            return new[]
            {
                new LanguageViewModel { Id = 1, Name = "C++" },
                new LanguageViewModel { Id = 2, Name = "C#" }
            };
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

            var submit = new Submit
            {
                ProblemId = problemId,
                LanguageId = selectedLanguage,
                UserId = userId,
                FileName = file.FileName,
                SourceCode = sourceCode
            };

            using (var unitOfWork = _factory.GetUnitOfWork(true))
            {
                var submitRepository = unitOfWork.GetRepository<ISubmitRepository>();
                submitRepository.Add(submit);
                unitOfWork.Commit();
            }
        }
    }
}
