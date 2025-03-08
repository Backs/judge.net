using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Judge.Checker;
using Judge.Compiler;
using Judge.Data;
using Judge.JudgeService.CustomCheckers;
using Judge.JudgeService.Settings;
using Judge.Model.CheckSolution;
using Judge.Model.Entities;
using Judge.Model.SubmitSolution;
using Judge.Runner;
using NLog;
using Configuration = Judge.Runner.Configuration;

namespace Judge.JudgeService;

internal sealed class JudgeServiceImplementation : IJudgeService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly ILogger logger;
    private readonly ICustomCheckerService customCheckerService;
    private readonly IProblemSettingsProvider problemSettingsProvider;

    private readonly string workingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];
    private readonly string storagePath = ConfigurationManager.AppSettings["StoragePath"];
    private readonly string runnerPath = ConfigurationManager.AppSettings["RunnerPath"];

    public JudgeServiceImplementation(
        IUnitOfWorkFactory unitOfWorkFactory,
        ICustomCheckerService customCheckerService,
        IProblemSettingsProvider problemSettingsProvider,
        ILogger logger)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.problemSettingsProvider = problemSettingsProvider;
        this.customCheckerService = customCheckerService;
        this.logger = logger;
    }

    private CompileResult Compile(Language language, string fileName, string sourceCode)
    {
        if (!File.Exists(language.CompilerPath))
        {
            this.logger.Error($"Compiler not found: {language.Name}, {language.CompilerPath}");
            return CompileResult.NotFound();
        }

        var compiler = new Compiler.Compiler(this.logger)
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
        return compiler.Compile(compileSource, this.workingDirectory);
    }

    private void CopyChecker(Task task)
    {
        var path = Path.Combine(this.storagePath, task.TestsFolder, "check.exe");
        if (File.Exists(path))
        {
            File.Copy(path, Path.Combine(this.workingDirectory, "check.exe"));
        }
    }

    public JudgeResult Check(SubmitResult submitResult)
    {
        this.CreateWorkingDirectory();

        Language language;
        Task task;
        using (var uow = this.unitOfWorkFactory.GetUnitOfWork())
        {
            language = uow.Languages.Get(submitResult.Submit.LanguageId);
            task = uow.Tasks.Get(submitResult.Submit.ProblemId);
        }

        CompileResult compileResult;
        if (language.IsCompilable)
        {
            compileResult = this.Compile(language, submitResult.Submit.FileName, submitResult.Submit.SourceCode);
            this.logger.Info($"Compile result: {compileResult.CompileStatus}");
        }
        else
        {
            File.WriteAllText(Path.Combine(this.workingDirectory, submitResult.Submit.FileName),
                submitResult.Submit.SourceCode);
            compileResult = CompileResult.GetEmpty(submitResult.Submit.FileName);
        }

        ICollection<SubmitRunResult> results = null;

        if (compileResult.CompileStatus == CompileStatus.Success)
        {
            results = this.customCheckerService.Check(submitResult, CheckerType.PreExecutable);
        }

        if (results == null && compileResult.CompileStatus == CompileStatus.Success)
        {
            results = this.GetSubmitRunResults(submitResult, language, task, compileResult.FileName);
        }

        var lastRunResult = results?.Last();

        this.RemoveWorkingDirectory();

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

    private ICollection<SubmitRunResult> GetSubmitRunResults(SubmitResult submitResult, Language language,
        Task task, string fileName)
    {
        var runString = GetRunString(language, fileName);
        this.logger.Info($"Run string: {runString}");

        this.CopyChecker(task);
        return this.Run(submitResult, task, runString);
    }

    private static string GetRunString(Language language, string compileResultFileName)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(compileResultFileName);

        return language.RunStringFormat.Replace(TemplateKeys.FileName, fileNameWithoutExtension);
    }

    private void RemoveWorkingDirectory()
    {
        if (Directory.Exists(this.workingDirectory))
        {
            //TODO: remove directory with retry
            try
            {
                Directory.Delete(this.workingDirectory, true);
            }
            catch
            {
                // ignored
            }
        }
    }

    private ICollection<SubmitRunResult> Run(SubmitResult submitResult, Task task, string runString)
    {
        var inputFiles = this.GetInputFiles(task);
        var results = new List<SubmitRunResult>(10);
        foreach (var input in inputFiles)
        {
            var runResult = this.Run(submitResult, task, input, runString);
            results.Add(runResult);
            if (!runResult.RunSuccess)
            {
                break;
            }
        }

        return results;
    }

    private SubmitRunResult Run(SubmitResult submitResult, Task task, string input, string runString)
    {
        var runService = new RunService(this.runnerPath, this.workingDirectory);
        var configuration = new Configuration(runString, this.workingDirectory, task.TimeLimitMilliseconds,
            task.MemoryLimitBytes);
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

        this.logger.Info($"Run status: {runResult.RunStatus}");

        if (runResult.RunStatus == RunStatus.Success)
        {
            var checkAnswerResult = this.CheckAnswer(submitResult, configuration);
            result.CheckMessage = checkAnswerResult.Message;
            result.CheckStatus = checkAnswerResult.CheckStatus;
        }

        return result;
    }

    private CheckResult CheckAnswer(SubmitResult submitResult, Configuration configuration)
    {
        var problemSettings = this.problemSettingsProvider.GetProblemSettings(submitResult.Submit);

        if (problemSettings?.SkipExecutableChecker == true)
        {
            var results = this.customCheckerService.Check(submitResult, CheckerType.PostExecutable);
            var result = results?.Last();

            if (result == null)
            {
                this.logger.Error($"SkipExecutableChecker is true, but no custom checkers were found. Submit id: {submitResult.Submit.Id}");
                return new CheckResult(CheckStatus.Fail);
            }

            return new CheckResult(result.CheckStatus);
        }

        var answerFileName = configuration.InputFile + ".a";
        var checkResult = ExecutableChecker.Check(this.workingDirectory, configuration.InputFile,
            configuration.OutputFile,
            answerFileName);

        this.logger.Info(
            $"Check result: {configuration.InputFile}, {checkResult.CheckStatus}, {checkResult.Message}");

        return checkResult;
    }

    private IEnumerable<string> GetInputFiles(Task task)
    {
        var path = Path.Combine(this.storagePath, task.TestsFolder);
        return Directory.EnumerateFiles(path, "*.", SearchOption.AllDirectories)
            .OrderBy(Path.GetFileName, FileNameComparer.Instance);
    }

    private void CreateWorkingDirectory()
    {
        this.RemoveWorkingDirectory();
        Directory.CreateDirectory(this.workingDirectory);
    }

    private sealed class FileNameComparer : IComparer<string>
    {
        public static FileNameComparer Instance { get; } = new FileNameComparer();

        public int Compare(string x, string y)
        {
            int.TryParse(x, out var ax);
            int.TryParse(y, out var ay);

            return ax.CompareTo(ay);
        }
    }
}