using System;

namespace Judge.Model.Contests;

public sealed class Contest
{
    public int Id { get; private set; }

    public string Name { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime FinishTime { get; set; }

    public DateTime? FreezeTime { get; set; }

    public bool IsOpened { get; set; }

    public ContestRules Rules { get; set; }

    public DateTime? CheckPointTime { get; set; }

    public bool OneLanguagePerTask { get; set; }

    public string Analysis { get; set; }

    public override string ToString()
    {
        return $"{this.Id}-{this.Name}";
    }
}