using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Judge.Application.ViewModels.Admin.Problems
{
    public sealed class EditProblemViewModel
    {
        public long? Id { get; set; }
        public bool IsNewTask => Id == null;

        [Required]
        [MaxLength(512)]
        [Display(Name = "Папка с тестами")]
        public string TestsFolder { get; set; }

        [Range(1, 10 * 60 * 1000)]
        [Display(Name = "Ограничение по времени в мс.")]
        public int TimeLimitMilliseconds { get; set; } = 1000;

        [Range(1, 10485760000)]
        [Display(Name = "Ограничение по памяти в байтах")]
        public int MemoryLimitBytes { get; set; } = 104857600;

        [Required]
        [MaxLength(256)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Описание задачи")]
        [AllowHtml]
        public string Statement { get; set; }

        [Display(Name = "Доступна в общем списке задач")]
        public bool IsOpened { get; set; }
    }
}
