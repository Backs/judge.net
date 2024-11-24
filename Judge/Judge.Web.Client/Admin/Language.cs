using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Admin;

/// <summary>
/// Language
/// </summary>
public class Language
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Language name
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Language description
    /// </summary>
    [Required]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Is language compilable
    /// </summary>
    [Required]
    public bool IsCompilable { get; set; }

    /// <summary>
    /// Compiler path
    /// </summary>
    public string? CompilerPath { get; set; }

    /// <summary>
    /// Compiler options template
    /// </summary>
    public string? CompilerOptionsTemplate { get; set; }

    /// <summary>
    /// Executable file template
    /// </summary>
    [Required]
    public string OutputFileTemplate { get; set; } = null!;

    /// <summary>
    /// Run string template
    /// </summary>
    [Required]
    public string RunStringTemplate { get; set; } = null!;

    /// <summary>
    /// Is hidden
    /// </summary>
    [Required]
    public bool IsHidden { get; set; }
}