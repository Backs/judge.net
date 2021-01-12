namespace Judge.Application.ViewModels.Admin.Problems
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public sealed class EditProblemViewModel
    {
        public long? Id { get; set; }
        public bool IsNewTask => this.Id == null;

        [Required]
        [MaxLength(512)]
        [Display(ResourceType = typeof(Resources), Name = "TestsFolder")]
        public string TestsFolder { get; set; }

        [Range(1, 10 * 60 * 1000)]
        [Display(ResourceType = typeof(Resources), Name = "TimeLimitMilliseconds")]
        public int TimeLimitMilliseconds { get; set; } = 1000;

        [Range(1, 10485760000)]
        [Display(ResourceType = typeof(Resources), Name = "MemoryLimitBytes")]
        public int MemoryLimitBytes { get; set; } = 104857600;

        [Required]
        [MaxLength(256)]
        [Display(ResourceType = typeof(Resources), Name = "TaskName")]
        public string Name { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "TaskStatement")]
        [AllowHtml]
        public string Statement { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TaskIsOpened")]
        public bool IsOpened { get; set; }
    }
}