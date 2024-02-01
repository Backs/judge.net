using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Judge.Web.Client.Contests;

public class EditContest : IValidatableObject
{
    public int? Id { get; set; }

    [Required] public string Name { get; set; } = null!;

    [Required] public DateTime StartTime { get; set; }

    [Required] public DateTime FinishTime { get; set; }
    public DateTime? CheckPointTime { get; set; }

    [Required] public ContestRules Rules { get; set; }
    public EditContestTask[] Tasks { get; set; } = Array.Empty<EditContestTask>();
    public bool OneLanguagePerTask { get; set; }
    public bool IsOpened { get; set; }

    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        var duplicated = this.Tasks.GroupBy(o => o.Label).Where(o => o.Count() > 1).Select(o => o.Key).ToArray();
        if (duplicated.Length != 0)
        {
            yield return new ValidationResult($"Duplicated labels: {string.Join("; ", duplicated)}");
        }
    }
}