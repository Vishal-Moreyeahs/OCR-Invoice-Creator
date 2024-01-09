using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OcrInvoice.Application.Models;

namespace OcrInvoice.Application.Contracts.Respository
{
    public interface IInvoiceCreateRepository
    {
        Task<ApiResponse<dynamic>> CreateInvoice(InvoiceOcrRequest request);
        Task<ApiResponse<dynamic>> UploadInvoiceWithInvoiceID(int invoiceId, IFormFile uploadInvoice);

    }
}
