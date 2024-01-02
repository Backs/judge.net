using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Judge.Web.Client.Submits;

public sealed class SubmitSolution : IValidatableObject
{
    [Required] public IFormFile File { get; set; } = null!;

    [Required] public int LanguageId { get; set; }

    public long? ProblemId { get; set; }

    public int? ContestId { get; set; }

    public string? TaskLabel { get; set; }

    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        if (this.ProblemId == null ^ !(this.ContestId == null && this.TaskLabel == null))
        {
            yield return new ValidationResult("You should set ProblemId or ContestId and TaskLabel");
        }
    }
}