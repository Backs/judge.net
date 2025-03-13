using System;
using System.Collections.Generic;
using Judge.Model.CheckSolution;
using Judge.Model.Entities;

namespace Judge.Model.SubmitSolution;

public abstract class SubmitBase
{
    public long UserId { get; set; }
    public User User { get; set; }
    public string FileName { get; set; }
    public int LanguageId { get; set; }
    public string SourceCode { get; set; }
    public long Id { get; internal set; }
    public ICollection<SubmitResult> Results { get; } = new HashSet<SubmitResult>();
    public DateTime SubmitDateUtc { get; private set; }
    public long ProblemId { get; set; }
    public Task Problem { get; set; }
    public string UserHost { get; set; }
    public string SessionId { get; set; }
}