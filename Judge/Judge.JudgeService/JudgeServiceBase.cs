using System.Configuration;
using System.IO;
using Judge.Model.Configuration;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;

namespace Judge.JudgeService
{
    internal abstract class JudgeServiceBase : IJudgeService
    {
        private readonly ILanguageRepository _languageRepository;

        private readonly string _workingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];

        protected JudgeServiceBase(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        private void Compile(Language language)
        {
            var compiler = new Compiler.Compiler
            {
                CompilerPath = language.CompilerPath,
                CompilerOptionsTemplate = language.CompilerOptionsTemplate,
                OutputFileTemplate = language.OutputFileTemplate
            };
        }

        public void Check(SubmitResult submitResult)
        {
            CreateWorkingDirectory();

            var language = _languageRepository.Get(submitResult.Submit.LanguageId);

            if (language.IsCompilable)
            {
            }
        }

        private void CreateWorkingDirectory()
        {
            if (Directory.Exists(_workingDirectory))
            {
                Directory.Delete(_workingDirectory);
            }
            Directory.CreateDirectory(_workingDirectory);
        }
    }
}
