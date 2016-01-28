using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Judge.Data
{
    internal interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
