using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Judge.Web.Client.Submits;

/// <summary>
/// Submit solution for problem
/// </summary>
public sealed class SubmitSolution : IValidatableObject
{
    /// <summary>
    /// Solution
    /// </summary>
    [Required]
    public string Solution { get; set; } = null!;

    /// <summary>
    /// Language id
    /// </summary>
    [Required]
    public int LanguageId { get; set; }

    /// <summary>
    /// problem id
    /// </summary>
    public long? ProblemId { get; set; }

    /// <summary>
    /// Contest id
    /// </summary>
    public int? ContestId { get; set; }

    /// <summary>
    /// Problem label in contest
    /// </summary>
    public string? ProblemLabel { get; set; }

    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        if (this.ProblemId == null ^ !(this.ContestId == null || this.ProblemLabel == null))
        {
            yield return new ValidationResult("You should set ProblemId or ContestId and TaskLabel",
                new[] { "submit" });
        }
    }
}