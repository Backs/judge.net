namespace Judge.Application.ViewModels
{
    public sealed class UserInfo
    {
        public UserInfo(long userId, string host, string sessionId)
        {
            UserId = userId;
            Host = host;
            SessionId = sessionId;
        }

        public long UserId { get; }
        public string Host { get; }
        public string SessionId { get; }
    }
}
