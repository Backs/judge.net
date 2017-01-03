using System.Diagnostics;

namespace Judge.Runner
{
    public sealed class RunService
    {
        private readonly string _runnerPath;
        private readonly string _workingDirectory;

        public RunService(string runnerPath, string workingDirectory)
        {
            _runnerPath = runnerPath;
            _workingDirectory = workingDirectory;
        }

        public void Run(Configuration configuration)
        {
            var startInfo = new ProcessStartInfo(_runnerPath, configuration.ToString())
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = _workingDirectory
            };

            var p = new Process { StartInfo = startInfo };

            p.Start();

            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }
    }
}
