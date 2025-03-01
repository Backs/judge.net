namespace Judge.Model.Entities
{
    public sealed class Language
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompilable { get; set; }
        public string CompilerPath { get; set; }
        public string CompilerOptionsTemplate { get; set; }
        public string OutputFileTemplate { get; set; }
        public string RunStringFormat { get; set; }
        public bool IsHidden { get; set; }
        public string DefaultFileName { get; set; }
        public bool AutoDetectFileName { get; set; }
    }
}