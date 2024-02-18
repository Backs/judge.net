using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Judge.Web.Client.Contests;

/// <summary>
/// Creates or edit contest
/// </summary>
public class EditContest : IValidatableObject
{
    /// <summary>
    /// Contest id
    /// </summary>
    /// <remarks>Creates new if empty</remarks>
    public int? Id { get; set; }

    /// <summary>
    /// Contest name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Contest start time
    /// </summary>
    [Required]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Contest finish time
    /// </summary>
    [Required]
    public DateTime FinishTime { get; set; }

    /// <summary>
    /// Contest check point
    /// </summary>
    /// <remarks>Applies only if Rules = CheckPoint</remarks>
    public DateTime? CheckPointTime { get; set; }

    /// <inheritdoc cref="Judge.Web.Client.Contests.ContestRules"/>
    [Required]
    public ContestRules Rules { get; set; }

    /// <summary>
    /// Contest problems
    /// </summary>
    public EditContestProblem[] Problems { get; set; } = Array.Empty<EditContestProblem>();

    /// <summary>
    /// If true, only one language can be used for every task
    /// </summary>
    public bool OneLanguagePerTask { get; set; }

    /// <summary>
    /// If contest is shown for participants
    /// </summary>
    public bool IsOpened { get; set; }

    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        var duplicated = this.Problems.GroupBy(o => o.Label).Where(o => o.Count() > 1).Select(o => o.Key).ToArray();
        if (duplicated.Length != 0)
        {
            yield return new ValidationResult($"Duplicated labels: {string.Join("; ", duplicated)}",
                new[] { nameof(this.Problems) });
        }

        if (this.Rules == ContestRules.CheckPoint ^ this.CheckPointTime != null)
        {
            yield return new ValidationResult("CheckPointTime is only allowed for CheckPoint rules",
                new[] { nameof(this.CheckPointTime), nameof(this.Rules) });
        }
    }
}