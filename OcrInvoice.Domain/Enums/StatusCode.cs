using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoice.Domain.Enums
{
    public enum StatusCode
    {
        BadRequest = 400,
        Success = 200,
        InternalServerError = 500
    }
}
