using System.Diagnostics;
using System.IO;

namespace Judge.Runner
{
    public sealed class RunService : IRunService
    {
        private readonly string _runnerPath;
        private readonly string _workingDirectory;

        public RunService(string runnerPath, string workingDirectory)
        {
            _runnerPath = runnerPath;
            _workingDirectory = workingDirectory;
        }

        public RunResult Run(Configuration configuration)
        {
            var startInfo = new ProcessStartInfo(_runnerPath, configuration.ToString())
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = _workingDirectory,
                CreateNoWindow = true,
                ErrorDialog = false
            };

            if (!Directory.Exists(_workingDirectory))
            {
                Directory.CreateDirectory(_workingDirectory);
            }

            string output;
            using (var p = new Process { StartInfo = startInfo })
            {
                p.Start();

                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            }

            return RunResult.Parse(output);
        }
    }
}
