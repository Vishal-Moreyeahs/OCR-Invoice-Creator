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

        Task<ApiResponse<dynamic>> UploadInvoice(IFormFile InvoiceFile);

        Task<ApiResponse<dynamic>> CreateAndUploadInvoice(InvoiceRequest invoice);

        Task<ApiResponse<dynamic>> GetAllInvoices();
        Task<ApiResponse<dynamic>> DeleteInvoice(int invoiceId);

        Task<ApiResponse<dynamic>> GetInvoiceImageByInvoiceId(int invoiceId);
    }
}
