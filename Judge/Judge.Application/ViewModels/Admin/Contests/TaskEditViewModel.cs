using System.ComponentModel.DataAnnotations;
using Judge.Model.Contests;

namespace Judge.Application.ViewModels.Admin.Contests
{
    public sealed class TaskEditViewModel
    {
        public TaskEditViewModel(ContestTask o)
        {
            ProblemId = o.Task.Id;
            Label = o.TaskName;
        }

        public TaskEditViewModel()
        {
            
        }

        [Required]
        public long? ProblemId { get; set; }

        [Required]
        [MaxLength(6)]
        [MinLength(1)]
        public string Label { get; set; }
    }
}
