namespace Judge.Application.ViewModels.Admin.Contests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public sealed class EditContestViewModel : IValidatableObject
    {
        public EditContestViewModel()
        {
            this.Tasks = new List<TaskEditViewModel>();
        }

        public bool IsNewContest => this.Id == null;

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "ContestName")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(Resources), Name = "ContestStart")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(Resources), Name = "ContestFinish")]
        public DateTime FinishTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(Resources), Name = "ContestCheckPointTime")]
        public DateTime? CheckPointTime { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ContestIsOpened")]
        public bool IsOpened { get; set; }

        public int? Id { get; set; }
        public List<TaskEditViewModel> Tasks { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Rules")]
        public ContestRules Rules { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Tasks.Any(o => string.IsNullOrWhiteSpace(o.Label)))
            {
                yield return new ValidationResult(Resources.TaskHaveNoLabel);

                yield break;
            }

            if (this.Tasks.GroupBy(o => o.ProblemId).Any(o => o.Count() > 1))
                yield return new ValidationResult(Resources.TasksHaveDuplicates);

            if (this.Tasks.GroupBy(o => o.Label).Any(o => o.Count() > 1))
                yield return new ValidationResult(Resources.TaskLabelsHaveDuplicates);
        }
    }
}