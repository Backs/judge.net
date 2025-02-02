using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Submits;

/// <summary>
/// Submit result information
/// </summary>
public sealed class SubmitResultExtendedInfo : SubmitResultInfo
{
    /// <summary>
    /// Source code of solution
    /// </summary>
    [Required]
    public string SourceCode { get; set; } = null!;
    
    /// <summary>
    /// Compiler output
    /// </summary>
    public string? CompilerOutput { get; set; }
    
    /// <summary>
    /// Text output of run.exe
    /// </summary>
    public string? RunOutput { get; set; }
    
    /// <summary>
    /// User host of submit
    /// </summary>
    public string? UserHost { get; set; }
}