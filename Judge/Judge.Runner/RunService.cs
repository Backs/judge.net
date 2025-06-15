using System.Diagnostics;
using System.IO;
using Judge.Runner.Abstractions;

namespace Judge.Runner;

public sealed class RunService : IRunService
{
    private readonly string runnerPath;
    private readonly string workingDirectory;

    public RunService(string runnerPath, string workingDirectory)
    {
        this.runnerPath = runnerPath;
        this.workingDirectory = workingDirectory;
    }

    public IRunResult Run(RunOptions options)
    {
        var configuration = new Configuration(options.Executable,
            options.WorkingDirectory, (int)options.TimeLimit.TotalMilliseconds, options.MemoryLimitBytes)
        {
            InputFile = options.Input,
            OutputFile = options.Output
        };

        return this.Run(configuration);
    }

    public IRunResult Run(Configuration configuration)
    {
        var startInfo = new ProcessStartInfo(this.runnerPath, configuration.ToString())
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = this.workingDirectory,
            CreateNoWindow = true,
            ErrorDialog = false
        };

        if (!Directory.Exists(this.workingDirectory))
        {
            Directory.CreateDirectory(this.workingDirectory);
        }

        string output;
        int exitCode;
        using (var p = new Process())
        {
            p.StartInfo = startInfo;
            p.Start();

            output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            exitCode = p.ExitCode;
        }

        return RunResult.Parse(output, exitCode);
    }
}