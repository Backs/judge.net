using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Admin;

public sealed class AllProblemInfo
{
    /// <summary>
    /// Problem id
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// Problem name
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Is problem opened for everyone
    /// </summary>
    [Required]
    public bool IsOpened { get; set; }
}