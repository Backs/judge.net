using System;
using System.Linq.Expressions;

namespace Judge.Model.CheckSolution
{
    public sealed class OpenedTasksSpecification : ISpecification<Task>
    {
        private OpenedTasksSpecification()
        {

        }

        public static OpenedTasksSpecification Instance { get; } = new OpenedTasksSpecification();

        public Expression<Func<Task, bool>> IsSatisfiedBy { get; } = x => x.IsOpened;
    }
}
