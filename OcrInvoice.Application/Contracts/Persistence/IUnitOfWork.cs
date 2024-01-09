using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoice.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> Save();
    }
}
