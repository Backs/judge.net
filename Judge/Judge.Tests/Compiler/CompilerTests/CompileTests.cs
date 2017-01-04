using System.IO;
using Judge.Compiler;
using NUnit.Framework;

namespace Judge.Tests.Compiler.CompilerTests
{
    [TestFixture]
    public class CompileTests
    {
        private readonly string _workingDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "WorkingDirectory");

        [Test]
        public void SuccessCompilerTest()
        {
            var compiler = new Judge.Compiler.Compiler
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
        }

        private static string LoadSourceCode(string fileName)
        {
            fileName = Path.Combine(TestContext.CurrentContext.TestDirectory, fileName);

            return File.ReadAllText(fileName);
        }
    }
}
