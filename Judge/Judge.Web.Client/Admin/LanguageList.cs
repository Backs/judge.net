using System;
using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Admin;

/// <summary>
/// Language list
/// </summary>
public sealed class LanguageList
{
    /// <summary>
    /// Languages
    /// </summary>
    [Required]
    public Language[] Items { get; set; } = Array.Empty<Language>();
}