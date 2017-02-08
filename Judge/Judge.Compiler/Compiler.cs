using System.Diagnostics;
using System.IO;

namespace Judge.Compiler
{
    public class Compiler : ICompiler
    {
        public string CompilerPath { get; set; }
        public string CompilerOptionsTemplate { get; set; }
        public string OutputFileTemplate { get; set; }
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

            var startInfo = new ProcessStartInfo(CompilerPath, options)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = workingDirectory,
                CreateNoWindow = true
            };

            string output;
            int exitCode;

            using (var p = new Process { StartInfo = startInfo })
            {
                p.Start();

                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit(30 * 1000);
                exitCode = p.ExitCode;
            }

            if (exitCode == 0)
            {
                return CompileResult.Success(output, OutputFileTemplate.Replace(TemplateKeys.FileName, fileNameWithoutExtension));
            }

            return CompileResult.Error(output);
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
