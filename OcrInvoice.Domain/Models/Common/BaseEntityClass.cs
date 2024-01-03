using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoice.Domain.Models.Common
{
    public class BaseEntityClass
    {
        [Required]
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set;}
    }
}
