namespace Judge.Model.Entities
{
    public sealed class UserRole
    {
        public int Id { get; private set; }
        public string RoleName { get; set; }
        public long UserId { get; set; }
    }
}
