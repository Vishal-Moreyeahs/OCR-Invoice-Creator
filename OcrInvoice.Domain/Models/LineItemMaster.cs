using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoice.Domain.Models
{
    public class LineItemMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LineItemId { get; set; }
        public int? InvoiceID { get; set; }
        public string? ItemName { get; set; }
        public int? Qty { get; set; }
        public double? Price { get; set; }
        public double? Tax { get; set; }
    }
}
