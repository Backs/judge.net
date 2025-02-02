using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Judge.Model.Entities;

namespace Judge.Model.Account
{
    public sealed class UserListSpecification : ISpecification<User>
    {
        public UserListSpecification(IEnumerable<long> users)
        {
            this.IsSatisfiedBy = u => users.Contains(u.Id);
        }

        public Expression<Func<User, bool>> IsSatisfiedBy { get; }
    }
}
