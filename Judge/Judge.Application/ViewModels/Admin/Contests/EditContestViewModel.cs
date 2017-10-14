using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Judge.Application.ViewModels.Problems.ProblemsList;

namespace Judge.Application.ViewModels.Admin.Contests
{
    public sealed class EditContestViewModel
    {
        public bool IsNewContest => Id == null;

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

        [Display(ResourceType = typeof(Resources), Name = "ContestIsOpened")]
        public bool IsOpened { get; set; }
        public int? Id { get; set; }
        public List<TaskEditViewModel> Tasks { get; set; }
    }
}
