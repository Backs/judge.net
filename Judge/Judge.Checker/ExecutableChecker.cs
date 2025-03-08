namespace Judge.Checker;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

public static class ExecutableChecker
{
    private const string CheckerFileName = "check.exe";

    public static CheckResult Check(string workingDirectory, string inputFileName, string outputFileName, string answerFileName)
    {
        if (!Directory.Exists(workingDirectory))
        {
            return CheckResult.Fail("Working directory not found");
        }

        var checker = Directory.GetFiles(workingDirectory, CheckerFileName).FirstOrDefault();

        if (checker == null)
        {
            return CheckResult.Fail("Checker not found");
        }

        var inputFile = Path.Combine(workingDirectory, inputFileName);
        var outputFile = Path.Combine(workingDirectory, outputFileName);
        var answerFile = Path.Combine(workingDirectory, answerFileName);

        string options = $"{inputFile} {outputFile} {answerFile}";

        var startInfo = new ProcessStartInfo(checker, options)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            WorkingDirectory = workingDirectory,
            CreateNoWindow = true,
            ErrorDialog = false
        };

        var exitCode = 1;
        var message = string.Empty;

        using (var p = new Process { StartInfo = startInfo })
        {
            try
            {
                p.Start();

                p.WaitForExit(10 * 1000);

                p.OutputDataReceived += (s, e) => { message = e.Data; };

                p.ErrorDataReceived += (s, e) => { message = e.Data; };

                if (p.HasExited)
                {
                    exitCode = p.ExitCode;
                }
            }
            catch
            {
                exitCode = 1;
            }
            finally
            {
                if (!p.HasExited)
                {
                    p.Kill();
                }
            }
        }

        var checkStatus = !Enum.IsDefined(typeof(CheckStatus), exitCode) ? CheckStatus.WA : (CheckStatus)exitCode;

        return new CheckResult(checkStatus, message);
    }
}