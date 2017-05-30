using System;
using System.Linq.Expressions;

namespace Judge.Model
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> IsSatisfiedBy { get; }
    }
}
