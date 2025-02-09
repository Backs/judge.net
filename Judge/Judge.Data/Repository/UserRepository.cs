using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Judge.Model;
using Judge.Model.Account;
using Judge.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Judge.Data.Repository;

internal sealed class UserRepository : IUserRepository
{
    private readonly DataContext context;

    public UserRepository(DataContext context)
    {
        this.context = context;
    }

    public void Add(User user)
    {
        this.context.Set<User>().Add(user);
    }

    public Task<User?> FindByEmailAsync(string email)
    {
        return this.BaseQuery().FirstOrDefaultAsync(o => o!.Email == email);
    }

    public Task<User?> FindByLoginASync(string login)
    {
        return this.BaseQuery().FirstOrDefaultAsync(o => o!.UserName == login);
    }

    public async Task<IReadOnlyCollection<User>> SearchAsync(ISpecification<User> specification, int skip = 0,
        int take = int.MaxValue)
    {
        IQueryable<User> query = this.BaseQuery().Where(specification.IsSatisfiedBy!)
            .OrderBy(o => o!.Id)!;

        if (skip != 0)
        {
            query = query.Skip(skip);
        }

        query = query.Take(take);

        return await query.ToListAsync();
    }

    public Task<int> CountAsync(ISpecification<User> specification)
    {
        return this.context.Set<User?>().CountAsync(specification.IsSatisfiedBy!);
    }

    public Task<User?> GetAsync(long id)
    {
        return this.BaseQuery().FirstOrDefaultAsync(o => o!.Id == id);
    }

    private IIncludableQueryable<User?, ICollection<UserRole>> BaseQuery()
    {
        return this.context.Set<User?>().Include(o => o!.UserRoles);
    }
}