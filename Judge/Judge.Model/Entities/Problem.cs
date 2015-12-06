namespace Judge.Model.Entities
{
    public sealed class Problem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public bool IsHidden { get; set; }
    }
}
