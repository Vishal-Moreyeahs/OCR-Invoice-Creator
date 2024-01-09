using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OcrInvoice.Domain.Models;

namespace OcrInvoice.Persistence
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataBaseContext).Assembly);
        }

        //Db Sets or tables
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<InvoiceMaster> InvoiceMasters { get; set; }
        public virtual DbSet<LineItemMaster> LineItemMasters { get; set; }

        public virtual DbSet<InvoiceImage> InvoiceImages { get; set; }
    }
}
