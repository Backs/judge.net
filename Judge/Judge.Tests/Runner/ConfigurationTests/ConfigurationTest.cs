using Judge.Runner;
using NUnit.Framework;

namespace Judge.Tests.Runner.ConfigurationTests
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        public void ToStringTest()
        {
            var configuration = new Configuration
            {
                AllowCreateProcesses = true,
                Directory = @"c:\temp",
                FileName = "main.exe",
                MemoryLimitBytes = 1234 * 1024,
                Quiet = true,
                ShowWindow = true,
                SingleCore = true,
                TimeLimitMilliseconds = 1500,
                InputFile = "input.txt",
                OutputFile = "output.txt"
            };

            var expected = "-i \"input.txt\" -o \"output.txt\" -t 1500ms -m 1234K -d \"c:\\temp\" -q -w -1 -Xacp \"main.exe\"";
            Assert.That(configuration.ToString(), Is.EqualTo(expected));
        }
    }
}
