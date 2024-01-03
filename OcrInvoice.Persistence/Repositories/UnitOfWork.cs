using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OcrInvoice.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace OcrInvoice.Persistence.Repositories
{
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _dbContext;

        public UnitOfWork(DataBaseContext context)
        {
            _dbContext = context;
        }

        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
