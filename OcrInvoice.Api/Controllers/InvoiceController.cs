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

        //[HttpPost]
        //[Route("updateInvoiceToDb")]
        //public async Task<IActionResult> UpdateInvoiceOcr(InvoiceOcrRequest invoice)
        //{
        //    return Ok(await _invoiceCreateRepository.CreateInvoice(invoice));
        //}

        //[HttpPost]
        //[Route("upload-invoice")]
        //public async Task<IActionResult> UploadInvoiceWithInvoiceID(int invoiceId, IFormFile uploadInvoice)
        //{
        //    return Ok(await _invoiceCreateRepository.UploadInvoiceWithInvoiceID(invoiceId, uploadInvoice));
        //}

        [HttpPost]
        [Route("upload-invoice-file")]
        public async Task<IActionResult> UploadInvoice(IFormFile file)
        {
            return Ok(await _invoiceCreateRepository.UploadInvoice(file));
        }

        [HttpPost]
        [Route("create-invoice")]
        public async Task<IActionResult> CreateInvoice([FromBody]InvoiceRequest invoice)
        {
            return Ok(await _invoiceCreateRepository.CreateAndUploadInvoice(invoice));
        }

        [HttpGet]
        [Route("get-invoice-list")]
        public async Task<IActionResult> GetAllInvoices()
        {
            return Ok(await _invoiceCreateRepository.GetAllInvoices());
        }

        //[HttpDelete]
        //[Route("delete-invoice-by-id")]
        //public async Task<IActionResult> DeleteInvoiceById(int invoiceId)
        //{
        //    return Ok(await _invoiceCreateRepository.DeleteInvoice(invoiceId));
        //}

        [HttpGet]
        [Route("get-invoice-image")]
        public async Task<IActionResult> GetInvoiceImage(int invoiceId)
        {
            return Ok(await _invoiceCreateRepository.GetInvoiceImageByInvoiceId(invoiceId));
        }
    }
}