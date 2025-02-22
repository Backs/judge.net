#nullable enable
using System;
using System.Linq.Expressions;

namespace Judge.Model.CheckSolution;

public sealed class TasksSpecification : ISpecification<Task>
{
    public TasksSpecification(string? name, bool openedOnly)
    {
        if (openedOnly)
        {
            this.IsSatisfiedBy = x => x.IsOpened;
        }

        if (name != null)
        {
            this.IsSatisfiedBy = this.IsSatisfiedBy.And(x => x.Name.Contains(name));
        }
    }

    public Expression<Func<Task, bool>> IsSatisfiedBy { get; } = x => true;
}