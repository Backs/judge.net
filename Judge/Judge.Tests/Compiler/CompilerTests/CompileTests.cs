using System.IO;
using Judge.Compiler;
using NLog;
using NUnit.Framework;
using Rhino.Mocks;

namespace Judge.Tests.Compiler.CompilerTests
{
    [TestFixture]
    public class CompileTests
    {
        private readonly string _workingDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "WorkingDirectory");

        [Test]
        public void SuccessCompilerTest()
        {
            var compiler = new Judge.Compiler.Compiler(MockRepository.GenerateMock<ILogger>())
            {
                CompilerPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe",
                CompilerOptionsTemplate = TemplateKeys.FileName + "." + TemplateKeys.FileNameExtension,
                OutputFileTemplate = TemplateKeys.FileName + ".exe"
            };

            var result = compiler.Compile(new CompileSource
            {
                FileName = "Program.cs",
                SourceCode = LoadSourceCode(@"TestSolutions\AB.cs")
            }, _workingDirectory);

            Assert.That(result.CompileStatus, Is.EqualTo(CompileStatus.Success));
            Assert.That(File.Exists(Path.Combine(_workingDirectory, result.FileName)));
        }

        [Test]
        public void CompilationErrorTest()
        {
            var compiler = new Judge.Compiler.Compiler(MockRepository.GenerateMock<ILogger>())
            {
                CompilerPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe",
                CompilerOptionsTemplate = TemplateKeys.FileName + "." + TemplateKeys.FileNameExtension,
                OutputFileTemplate = TemplateKeys.FileName + ".exe"
            };

            var result = compiler.Compile(new CompileSource
            {
                FileName = "Program.cs",
                SourceCode = LoadSourceCode(@"TestSolutions\ABCE.cs")
            }, _workingDirectory);

            Assert.That(result.CompileStatus, Is.EqualTo(CompileStatus.Error));
        }

        private static string LoadSourceCode(string fileName)
        {
            fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, fileName);

            return File.ReadAllText(fileName);
        }
    }
}
