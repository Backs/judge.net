#nullable enable
using System;
using System.Linq.Expressions;
using Judge.Model.Entities;

namespace Judge.Model.Account;

public sealed class UsersSpecification : ISpecification<User>
{
    public Expression<Func<User, bool>> IsSatisfiedBy { get; }

    public UsersSpecification(string? name)
    {
        if (name != null)
        {
            this.IsSatisfiedBy = x => x.UserName.Contains(name) || x.Email.Contains(name);
        }
        else
        {
            this.IsSatisfiedBy = _ => true;
        }
    }
}