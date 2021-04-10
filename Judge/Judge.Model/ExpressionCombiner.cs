namespace Judge.Model
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class ExpressionCombiner
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> newExp)
        {
            // get the visitor
            var visitor = new ParameterUpdateVisitor(newExp.Parameters.First(), exp.Parameters.First());
            // replace the parameter in the expression just created
            newExp = visitor.Visit(newExp) as Expression<Func<T, bool>>;

            // now you can and together the two expressions
            var binExp = Expression.And(exp.Body, newExp.Body);
            // and return a new lambda, that will do what you want. NOTE that the binExp has reference only to te newExp.Parameters[0] (there is only 1) parameter, and no other
            return Expression.Lambda<Func<T, bool>>(binExp, newExp.Parameters);
        }

        private sealed class ParameterUpdateVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression oldParameter;
            private readonly ParameterExpression newParameter;

            public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                this.oldParameter = oldParameter;
                this.newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (object.ReferenceEquals(node, this.oldParameter))
                    return this.newParameter;

                return base.VisitParameter(node);
            }
        }
    }
}