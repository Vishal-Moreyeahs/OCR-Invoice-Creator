using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OcrInvoice.Application.Contracts.Persistence;
using OcrInvoice.Application.Contracts.Respository;
using OcrInvoice.Application.Models;
using OcrInvoice.Domain.Models;

namespace OcrInvoice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceCreateRepository _invoiceCreateRepository;

        public InvoiceController(IInvoiceCreateRepository invoiceCreateRepository)
        {
            _invoiceCreateRepository = invoiceCreateRepository;
        }

        [HttpPost]
        [Route("updateInvoiceToDb")]
        public async Task<IActionResult> UpdateInvoiceOcr(InvoiceOcrRequest invoice)
        {
            return Ok(await _invoiceCreateRepository.CreateInvoice(invoice));
        }

        [HttpPost]
        [Route("upload-invoice")]
        public async Task<IActionResult> UploadInvoiceWithInvoiceID(int invoiceId, IFormFile uploadInvoice)
        {
            return Ok(await _invoiceCreateRepository.UploadInvoiceWithInvoiceID(invoiceId, uploadInvoice));
        }
    }
}

