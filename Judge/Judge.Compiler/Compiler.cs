using System.Diagnostics;
using System.IO;
using System.Text;
using NLog;

namespace Judge.Compiler
{
    public class Compiler : ICompiler
    {
        public string CompilerPath { get; set; }
        public string CompilerOptionsTemplate { get; set; }
        public string OutputFileTemplate { get; set; }

        private readonly ILogger logger;

        public Compiler(ILogger logger)
        {
            this.logger = logger;
        }

        public CompileResult Compile(CompileSource sourceCode, string workingDirectory)
        {
            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
            }

            CreateFile(sourceCode, workingDirectory);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceCode.FileName);
            var fileExtension = Path.GetExtension(sourceCode.FileName).Substring(1);

            var options = CompilerOptionsTemplate.Replace(TemplateKeys.FileName, fileNameWithoutExtension)
                                                    .Replace(TemplateKeys.FileNameExtension, fileExtension);

            this.logger.Info($"Compile options: {options}");

            var startInfo = new ProcessStartInfo(CompilerPath, options)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory,
                CreateNoWindow = true,
                ErrorDialog = false
            };

            var output = new StringBuilder();
            int exitCode;

            using (var p = new Process { StartInfo = startInfo })
            {
                p.OutputDataReceived += (s, e) =>
                {
                    output.AppendLine(e.Data);
                };

                p.ErrorDataReceived += (s, e) =>
                {
                    output.AppendLine(e.Data);
                };
                
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                p.WaitForExit(30 * 1000);
                exitCode = p.ExitCode;
            }

            if (exitCode == 0)
            {
                return CompileResult.Success(output.ToString(), OutputFileTemplate.Replace(TemplateKeys.FileName, fileNameWithoutExtension));
            }

            return CompileResult.Error(output.ToString());
        }

        private static void CreateFile(CompileSource sourceCode, string workingDirectory)
        {
            var filePath = Path.Combine(workingDirectory, sourceCode.FileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(sourceCode.SourceCode);
            }
        }
    }
}
