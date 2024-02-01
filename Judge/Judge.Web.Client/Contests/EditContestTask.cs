using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

public class EditContestTask
{
    public int ProblemId { get; set; }
    [Required]
    public string Label { get; set; } = null!;
}