using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Judge.Compiler;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Judge.Runner;

namespace Judge.JudgeService
{
    internal sealed class JudgeServiceImplementation : IJudgeService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ITaskRepository _taskRepository;

        private readonly string _workingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];
        private readonly string _storagePath = ConfigurationManager.AppSettings["StoragePath"];
        private readonly string _runnerPath = ConfigurationManager.AppSettings["RunnnerPath"];

        public JudgeServiceImplementation(ILanguageRepository languageRepository, ITaskRepository taskRepository)
        {
            _languageRepository = languageRepository;
            _taskRepository = taskRepository;
        }

        private CompileResult Compile(Language language, string fileName, string sourceCode)
        {
            var compiler = new Compiler.Compiler
            {
                CompilerPath = language.CompilerPath,
                CompilerOptionsTemplate = language.CompilerOptionsTemplate,
                OutputFileTemplate = language.OutputFileTemplate
            };

            var compileSource = new CompileSource
            {
                FileName = fileName,
                SourceCode = sourceCode
            };
            return compiler.Compile(compileSource, _workingDirectory);
        }

        public void Check(SubmitResult submitResult)
        {
            CreateWorkingDirectory();

            var language = _languageRepository.Get(submitResult.Submit.LanguageId);
            var task = _taskRepository.Get(submitResult.Submit.ProblemId);

            if (language.IsCompilable)
            {
                var result = Compile(language, submitResult.Submit.FileName, submitResult.Submit.SourceCode);

                Run(task, result.FileName);
            }
        }

        private void Run(Task task, string fileName)
        {
            var inputFiles = GetInputFiles(task);

            foreach (var input in inputFiles)
            {
                Run(task, input, fileName);
            }
        }

        private RunResult Run(Task task, string input, string fileName)
        {
            var runService = new RunService(_runnerPath, _workingDirectory);
            var configuration = new Runner.Configuration(fileName, _workingDirectory, task.TimeLimitMilliseconds, task.MemoryLimitBytes);
            configuration.InputFile = input;
            configuration.OutputFile = "output.txt"; //TODO

            return runService.Run(configuration);
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
