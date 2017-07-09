using System.ComponentModel.DataAnnotations;

namespace Judge.Application.ViewModels.Admin.Languages
{
    public sealed class LanguageEditViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompilable { get; set; }
        public string CompilerPath { get; set; }
        public string CompilerOptionsTemplate { get; set; }
        [Required]
        public string OutputFileTemplate { get; set; }
        [Required]
        public string RunStringFormat { get; set; }
        public bool IsHidden { get; set; }
        public int Id { get; set; }
    }
}
