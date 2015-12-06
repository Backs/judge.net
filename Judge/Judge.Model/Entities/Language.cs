namespace Judge.Model.Entities
{
    public sealed class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompilable { get; set; }
        public string CompileStringFormat { get; set; }
        public string RunStringFormat { get; set; }
        public bool IsHidden { get; set; }
    }
}
