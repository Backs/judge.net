namespace Judge.Checker
{
    public sealed class CheckResult
    {
        public CheckResult(CheckStatus checkStatus, string message)
            : this(checkStatus)
        {
            Message = message;
        }

        public CheckResult(CheckStatus checkStatus)
        {
            CheckStatus = checkStatus;
        }

        public CheckStatus CheckStatus { get; }
        public string Message { get; }

        public static CheckResult Fail(string message)
        {
            return new CheckResult(CheckStatus.Fail, message);
        }

    }
}
