using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoice.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);

        Task<bool> Add(T entity);

        Task<bool> AddRange(IEnumerable<T> entities);

        Task<bool> Delete(int id);

        Task<bool> Upsert(T entity);
    }
}
