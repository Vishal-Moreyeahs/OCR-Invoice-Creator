using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcrInvoice.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace OcrInvoice.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataBaseContext _context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(
            DataBaseContext context
        )
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public virtual async Task<bool> Add(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public virtual async Task<bool> AddRange(IEnumerable<T> entities)
        {
            try
            {
                await dbSet.AddRangeAsync(entities);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public virtual async Task<bool> Delete(int id)
        {
            try
            {
                var entity = await dbSet.FindAsync(id);
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public virtual async Task<bool> Upsert(T entity)
        {
            try
            {
                if (entity != null)
                {
                    dbSet.Update(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
