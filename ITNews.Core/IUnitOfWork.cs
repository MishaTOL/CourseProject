using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ITNews.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
