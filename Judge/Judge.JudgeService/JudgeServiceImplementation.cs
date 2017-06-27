using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Judge.Checker;
using Judge.Compiler;
using Judge.Model.CheckSolution;
using Judge.Model.Configuration;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Judge.Runner;
using Configuration = Judge.Runner.Configuration;

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

        private void CopyChecker(Task task)
        {
            var path = Path.Combine(_storagePath, task.TestsFolder, "check.exe");
            if (File.Exists(path))
            {
                File.Copy(path, Path.Combine(_workingDirectory, "check.exe"));
            }
        }

        public JudgeResult Check(SubmitResult submitResult)
        {
            CreateWorkingDirectory();

            var language = _languageRepository.Get(submitResult.Submit.LanguageId);
            var task = _taskRepository.Get(submitResult.Submit.ProblemId);

            CompileResult compileResult;
            if (language.IsCompilable)
            {
                compileResult = Compile(language, submitResult.Submit.FileName, submitResult.Submit.SourceCode);
            }
            else
            {
                File.WriteAllText(Path.Combine(_workingDirectory, submitResult.Submit.FileName), submitResult.Submit.SourceCode);
                compileResult = CompileResult.GetEmpty(submitResult.Submit.FileName);
            }

            ICollection<SubmitRunResult> results = null;
            SubmitRunResult lastRunResult = null;

            if (compileResult.CompileStatus == CompileStatus.Success)
            {
                var runString = GetRunString(language, compileResult.FileName);

                CopyChecker(task);
                results = Run(task, runString);
                lastRunResult = results.Last();
            }

            RemoveWorkingDirectory();

            return new JudgeResult
            {
                CompileResult = compileResult,
                RunStatus = lastRunResult?.RunStatus,
                Description = lastRunResult?.Description,
                Output = lastRunResult?.Output,
                TextStatus = lastRunResult?.TextStatus,
                PeakMemoryBytes = results?.Max(o => o.PeakMemoryBytes),
                TimeConsumedMilliseconds = results?.Max(o => o.TimeConsumedMilliseconds),
                TimePassedMilliseconds = results?.Max(o => o.TimePassedMilliseconds),
                TestRunsCount = results?.Count ?? 0,
                CheckStatus = lastRunResult?.CheckStatus
            };
        }

        private static string GetRunString(Language language, string compileResultFileName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(compileResultFileName);

            return language.RunStringFormat.Replace(TemplateKeys.FileName, fileNameWithoutExtension);
        }

        private void RemoveWorkingDirectory()
        {
            if (Directory.Exists(_workingDirectory))
            {
                //TODO: remove directory with retry
                try
                {
                    Directory.Delete(_workingDirectory, true);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private ICollection<SubmitRunResult> Run(Task task, string runString)
        {
            var inputFiles = GetInputFiles(task);
            var results = new List<SubmitRunResult>(10);
            foreach (var input in inputFiles)
            {
                var runResult = Run(task, input, runString);
                results.Add(runResult);
                if (!runResult.RunSuccess)
                {
                    break;
                }
            }
            return results;
        }

        private SubmitRunResult Run(Task task, string input, string runString)
        {
            var runService = new RunService(_runnerPath, _workingDirectory);
            var configuration = new Configuration(runString, _workingDirectory, task.TimeLimitMilliseconds, task.MemoryLimitBytes);
            configuration.InputFile = input;
            configuration.OutputFile = "output.txt"; //TODO

            var runResult = runService.Run(configuration);

            var result = new SubmitRunResult
            {
                Description = runResult.Description,
                Output = runResult.Output,
                PeakMemoryBytes = runResult.PeakMemoryBytes,
                RunStatus = runResult.RunStatus,
                TextStatus = runResult.TextStatus,
                TimeConsumedMilliseconds = runResult.TimeConsumedMilliseconds,
                TimePassedMilliseconds = runResult.TimePassedMilliseconds
            };

            if (runResult.RunStatus == RunStatus.Success)
            {
                var checkAnswerResult = CheckAnswer(configuration);
                result.CheckMessage = checkAnswerResult.Message;
                result.CheckStatus = checkAnswerResult.CheckStatus;
            }

            return result;
        }

        private CheckResult CheckAnswer(Configuration configuration)
        {
            var checker = new Checker.Checker();
            string answerFileName = configuration.InputFile + ".a";
            return checker.Check(_workingDirectory, configuration.InputFile, configuration.OutputFile, answerFileName);
        }

        private IEnumerable<string> GetInputFiles(Task task)
        {
            var path = Path.Combine(_storagePath, task.TestsFolder);
            return Directory.EnumerateFiles(path, "*.", SearchOption.AllDirectories);
        }

        private void CreateWorkingDirectory()
        {
            RemoveWorkingDirectory();
            Directory.CreateDirectory(_workingDirectory);
        }
    }
}
