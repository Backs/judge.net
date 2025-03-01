using System.Collections.Generic;
using Judge.Model.Entities;

namespace Judge.Services;

public interface IFileNameResolver
{
    string Resolve(string solution, int languageId, IEnumerable<Language> languages);
}