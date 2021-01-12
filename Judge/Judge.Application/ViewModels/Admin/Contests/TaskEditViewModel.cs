namespace Judge.Application.ViewModels.Admin.Contests
{
    using System.ComponentModel.DataAnnotations;
    using Judge.Model.Contests;

    public sealed class TaskEditViewModel
    {
        public TaskEditViewModel(ContestTask o)
        {
            this.ProblemId = o.Task.Id;
            this.Label = o.TaskName;
        }

        public TaskEditViewModel()
        {
        }

        [Required] public long? ProblemId { get; set; }

        [Required]
        [MaxLength(6)]
        [MinLength(1)]
        public string Label { get; set; }
    }
}