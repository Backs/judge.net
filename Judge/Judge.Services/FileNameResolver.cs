using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Judge.Model.Entities;

namespace Judge.Services;

public sealed class FileNameResolver : IFileNameResolver
{
    private static readonly Regex ClassNameRegex =
        new Regex(@"class (\w*)\s*{", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Resolve(string solution, int languageId, IEnumerable<Language> languages)
    {
        var language = languages.FirstOrDefault(o => o.Id == languageId);

        if (language == null)
        {
            throw new InvalidOperationException($"Language not found. Language id: {languageId}");
        }

        if (!string.IsNullOrWhiteSpace(language.DefaultFileName))
        {
            return language.DefaultFileName;
        }

        var match = ClassNameRegex.Match(solution);

        if (!match.Success || match.Groups.Count < 1)
        {
            return "main.tmp";
        }

        var className = match.Groups[1].Value;

        var filename = language.CompilerOptionsTemplate.Replace("{FileName}", className);

        return filename;
    }
}