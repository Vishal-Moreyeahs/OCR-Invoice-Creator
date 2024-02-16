using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoice.Domain.Models
{
    public class InvoiceMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceID { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? ProviderName { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public DateTime? Date { get; set; }
        public string? InvoiceCategory { get; set; }
        public string? CategoryId { get; set; }
        public string? TaxId { get; set; }
        public double? Total { get; set; }
        public double? TotalTaxes { get; set; }
        public double? CGST { get; set; }
        public double? SGST { get; set; }
        public double? GST { get; set; }
        public double? CgstPercentage { get; set; }
        public double? SgstPercentage { get; set; }
        public double? GstPercentage { get; set; }
        public string? ImageUrl { get; set; }
        public string? OcrPercentage { get; set; }
        public string? ProjectName { get; set; }
        public string? Status { get; set;}
        public string? Comments { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
