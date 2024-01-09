using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoice.Application.Models
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public T? Data { get; set; }
    }
}
