using Judge.Runner;
using NUnit.Framework;

namespace Judge.Tests.Runner.RunResultTests
{
    [TestFixture]
    public class ParseTests
    {
        [Test]
        public void TimeLimitExceededTest()
        {
            var input = @"Running ""TL.exe"", press ESC to terminate...
Time limit exceeded
Program failed to terminate within 1.00 sec
  time consumed: 1.09 of 1.00 sec
  time passed: 1.15 sec
peak memory: 3018752 of 10485760 bytes";

            var result = RunResult.Parse(input, 0);

            Assert.That(result.PeakMemoryBytes, Is.EqualTo(3018752));
            Assert.That(result.TimePassedMilliseconds, Is.EqualTo(1150));
            Assert.That(result.TimeConsumedMilliseconds, Is.EqualTo(1090));
            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.TimeLimitExceeded));
        }

        [Test]
        public void SecurityViolationTest()
        {
            var input = @"Running ""cmd"", press ESC to terminate...
Security violation: Child process created
  time consumed: 0.00 of 1.00 sec
  time passed: 0.01 sec
peak memory: 1376256 of 10485760 bytes";

            var result = RunResult.Parse(input, 0);

            Assert.That(result.PeakMemoryBytes, Is.EqualTo(1376256));
            Assert.That(result.TimePassedMilliseconds, Is.EqualTo(10));
            Assert.That(result.TimeConsumedMilliseconds, Is.EqualTo(0));
            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.SecurityViolation));
        }

        [Test]
        public void MemoryLimitExceededTest()
        {
            var input = @"Running ""notepad"", press ESC to terminate...
Memory limit exceeded
Program tried to allocate more than 10240 bytes
  time consumed: 0.00 of 1.00 sec
  time passed: 0.00 sec
peak memory: 90112 of 10240 bytes";

            var result = RunResult.Parse(input, 0);

            Assert.That(result.PeakMemoryBytes, Is.EqualTo(90112));
            Assert.That(result.TimePassedMilliseconds, Is.EqualTo(0));
            Assert.That(result.TimeConsumedMilliseconds, Is.EqualTo(0));
            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.MemoryLimitExceeded));
        }

        [Test]
        public void InvalidExitCodeTest()
        {
            var input = @"Running ""InvalidReturnCode.exe"", press ESC to terminate...
Program successfully terminated
  exit code: 1
  time consumed: 0.01 of 1.00 sec
  time passed: 0.05 sec
peak memory: 3670016 of 10485760 bytes";

            var result = RunResult.Parse(input, 1);

            Assert.That(result.PeakMemoryBytes, Is.EqualTo(3670016));
            Assert.That(result.TimePassedMilliseconds, Is.EqualTo(50));
            Assert.That(result.TimeConsumedMilliseconds, Is.EqualTo(10));
            Assert.That(result.RunStatus, Is.EqualTo(RunStatus.RuntimeError));
        }
    }
}
