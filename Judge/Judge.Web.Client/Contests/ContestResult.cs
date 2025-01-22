using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Contests;

public class ContestResult : Contest
{
    /// <inheritdoc cref="Judge.Web.Client.Contests.ContestUserResult"/>
    [Required]
    public ContestUserResult[] Users { get; set; } = Array.Empty<ContestUserResult>();
}