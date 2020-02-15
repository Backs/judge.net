using System;
using System.Linq.Expressions;
using Judge.Model.Entities;

namespace Judge.Model.Account
{
    public sealed class AllUsersSpecification : ISpecification<User>
    {
        public static ISpecification<User> Instance { get; } = new AllUsersSpecification();

        private AllUsersSpecification()
        {
            
        }
        public Expression<Func<User, bool>> IsSatisfiedBy { get; } = u => true;
    }
}
