using System;

namespace Judge.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        T GetRepository<T>() where T : class;
    }
}
