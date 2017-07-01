namespace Judge.Model.Entities
{
    public sealed class UserRole
    {
        public int Id { get; private set; }
        public User User { get; set; }
        public string RoleName { get; set; }
    }
}
