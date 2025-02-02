#nullable enable
using System;
using System.Linq.Expressions;

namespace Judge.Model.CheckSolution
{
    public sealed class OpenedTasksSpecification : ISpecification<Task>
    {
        public OpenedTasksSpecification() : this(null)
        {
        }

        public OpenedTasksSpecification(string? name)
        {
            this.IsSatisfiedBy = x => x.IsOpened;
            if (name != null)
            {
                this.IsSatisfiedBy = this.IsSatisfiedBy.And(x => x.Name.Contains(name));
            }
        }

        public Expression<Func<Task, bool>> IsSatisfiedBy { get; }
    }
}