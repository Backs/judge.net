namespace Judge.Web.Client.Problems;

/// <summary>
/// Problem info
/// </summary>
public sealed class ProblemInfo
{
    /// <summary>
    /// Problem id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Problem name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Is problem solve by current user
    /// </summary>
    public bool Solved { get; set; }
}