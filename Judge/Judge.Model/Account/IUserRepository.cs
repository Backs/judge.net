#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using Judge.Model.Entities;

namespace Judge.Model.Account;

public interface IUserRepository
{
    void Add(User user);

    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByLoginASync(string login);

    Task<IReadOnlyCollection<User>> SearchAsync(ISpecification<User> specification, int skip = 0, int take = int.MaxValue);
    Task<int> CountAsync(ISpecification<User> specification);
    Task<User?> GetAsync(long id);
}