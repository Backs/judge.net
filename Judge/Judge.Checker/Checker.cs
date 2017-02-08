using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Judge.Checker
{
    public sealed class Checker
    {
        private static string _checkerFileName = "check.exe";

        public CheckResult Check(string workingDirectory, string inputFileName, string outputFileName, string answerFileName)
        {
            if (!Directory.Exists(workingDirectory))
            {
                return CheckResult.Fail("Working directory not found");
            }

            var checker = Directory.GetFiles(workingDirectory, _checkerFileName).FirstOrDefault();

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
                CreateNoWindow = true
            };

            CheckStatus exitCode;
            string message;

            using (var p = new Process { StartInfo = startInfo })
            {
                p.Start();

                message = p.StandardOutput.ReadToEnd();
                p.WaitForExit(30 * 1000);
                exitCode = (CheckStatus)p.ExitCode;
            }


            return new CheckResult(exitCode, message);
        }
    }
}
