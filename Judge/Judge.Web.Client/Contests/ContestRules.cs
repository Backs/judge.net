namespace Judge.Web.Client.Contests;

/// <summary>
/// Contest rules
/// </summary>
public enum ContestRules
{
    /// <summary>
    /// Classic ACM-like rules
    /// </summary>
    Acm,

    /// <summary>
    /// Points-based results
    /// </summary>
    Points,

    /// <summary>
    /// Acm rules, but the penalty is calculated relative to CheckPoint
    /// </summary>
    CheckPoint,

    /// <summary>
    /// Dynamic points
    /// </summary>
    Dynamic
}