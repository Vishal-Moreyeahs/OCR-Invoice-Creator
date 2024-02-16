using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoice.Application.Models
{
    public class InvoiceRequest
    {
        public string? Title { get; set; }
        public string? MerchantName { get; set; }
        public string? BillNo { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public List<Product>? Products { get; set; }
        public string? TotalTaxes { get; set; }
        public string? ImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public string? OcrPercentage { get; set; }
        public string? ProjectName { get; set; }
        public string? Comments { get; set; }
        public string? TotalAmount { get; set; }
        public string? Status { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class Product
    {
        public string? ProductName { get; set; }
        public string? ProductAmount { get; set; }
    }

    public class InvoiceResponse : InvoiceRequest
    { 
        public int Id { get; set; }
    }

}
