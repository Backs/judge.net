using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;

namespace Judge.JudgeService
{
    internal abstract class JudgeServiceBase : IJudgeService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ITaskRepository _taskRepository;

        private readonly string _workingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];
        private readonly string _storagePath = ConfigurationManager.AppSettings["StoragePath"];

        protected JudgeServiceBase(ILanguageRepository languageRepository, ITaskRepository taskRepository)
        {
            _languageRepository = languageRepository;
            _taskRepository = taskRepository;
        }

        private void Compile(Language language, string fileName, string sourceCode)
        {
            var compiler = new Compiler.Compiler
            {
                CompilerPath = language.CompilerPath,
                CompilerOptionsTemplate = language.CompilerOptionsTemplate,
                OutputFileTemplate = language.OutputFileTemplate
            };

            var compileSource = new Compiler.CompileSource
            {
                FileName = fileName,
                SourceCode = sourceCode
            };
            compiler.Compile(compileSource, _workingDirectory);
        }

        public void Check(SubmitResult submitResult)
        {
            CreateWorkingDirectory();

            var language = _languageRepository.Get(submitResult.Submit.LanguageId);
            var task = _taskRepository.Get(submitResult.Submit.ProblemId);

            if (language.IsCompilable)
            {
                Compile(language, submitResult.Submit.FileName, submitResult.Submit.SourceCode);
            }

            Run(task);
        }

        private void Run(Task task)
        {
            var inputFiles = GetInputFiles(task);

            foreach (var input in inputFiles)
            {
                Run(input);
            }

        }

        private void Run(string input)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<string> GetInputFiles(Task task)
        {
            var path = Path.Combine(_storagePath, task.TestsFolder);
            return Directory.EnumerateFiles(path, "*.", SearchOption.AllDirectories);
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
