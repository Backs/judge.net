namespace Judge.Model.Entities
{
    public sealed class UserRole
    {
        public long Id { get; private set; }
        public string RoleName { get; set; }
        public long UserId { get; set; }
    }
}
