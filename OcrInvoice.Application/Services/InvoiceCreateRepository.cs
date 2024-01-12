using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using OcrInvoice.Application.Contracts.Persistence;
using OcrInvoice.Application.Contracts.Respository;
using OcrInvoice.Application.Models;
using OcrInvoice.Domain.Enums;
using OcrInvoice.Domain.Models;

namespace OcrInvoice.Application.Services
{
    public class InvoiceCreateRepository : IInvoiceCreateRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceCreateRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<dynamic>> CreateInvoice(InvoiceOcrRequest invoice)
        {
            var lineItemMasters = new List<LineItemMaster>();
            if (invoice == null || invoice.Worksheet1.Count == 0 || invoice.Worksheet2.Count == 0)
            {
                throw new ApplicationException($"Request Body is incorrect");
            }
            var data = invoice.Worksheet1.FirstOrDefault();
            
            var customer = _mapper.Map<Customer>(data);
            //var customer = new Customer
            //{
            //    Name = data.ReceiverName,
            //    Address = data.ReceiverAddress,
            //    TaxId = data.ReceiverGSTNumber
            //};
            var invoiceMaster = _mapper.Map<InvoiceMaster>(data);
            //var invoicemaster = new invoicemaster
            //{
            //    invoicenumber = data.invoicenumber,
            //    date = datetime.parse(data.invoicedate),
            //    address = data.senderaddress,
            //    providername = data.sendername,
            //    taxid = data.sendergstnumber,
            //    total = data.totalamount,
            //    ocrpercentage = data.ocrpercentage
            //};

            var isCustomerAdded = await _unitOfWork.GetRepository<Customer>().Add(customer);
            var isInvoiceMasterAdded = await _unitOfWork.GetRepository<InvoiceMaster>().Add(invoiceMaster);

            if (isCustomerAdded && isInvoiceMasterAdded)
            {
                await _unitOfWork.Save();
                foreach (var item in invoice.Worksheet2)
                {
                    lineItemMasters.Add(new LineItemMaster
                    {
                        ItemName = item.ItemName,
                        Price = Convert.ToDouble(item.ItemRate),
                        InvoiceID = invoiceMaster.InvoiceID,
                        Qty = item.ItemQuantity,
                        Tax = Convert.ToDouble(item.ItemRate)
                    });
                }
                var isLineItemsAdded = await _unitOfWork.GetRepository<LineItemMaster>().AddRange(lineItemMasters);
                if (isLineItemsAdded)
                {
                    await _unitOfWork.Save();
                    return new ApiResponse<dynamic>
                    {
                        StatusCode = (int)StatusCode.Success,
                        Success = true,
                        Data = new { InvoiceId = invoiceMaster.InvoiceID }
                    };
                }
                else
                {
                    throw new ApplicationException("Line Items not added");
                }
            }
            else
            {
                throw new ApplicationException("Data can not added in database");
            }

        }

        public async Task<ApiResponse<dynamic>> UploadInvoiceWithInvoiceID(int invoiceId, IFormFile uploadInvoice)
        {
            if (uploadInvoice == null || uploadInvoice.Length == 0)
            {
                throw new ApplicationException("No file uploaded.");
            }

            // Read the file data into a byte array
            byte[] photoData;
            using (var memoryStream = new MemoryStream())
            {
                await uploadInvoice.CopyToAsync(memoryStream);
                photoData = memoryStream.ToArray();
            }

            //var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");

            //// Check if the directory exists, and create it if it doesn't
            //if (!Directory.Exists(directoryPath))
            //{
            //    Directory.CreateDirectory(directoryPath);
            //}

            //var fileName = Path.Combine(directoryPath, string.Concat("invoice-",Path.GetRandomFileName()));

            //// Use FileStream to directly save the file
            //using (var fileStream = new FileStream(fileName, FileMode.Create))
            //{
            //    await uploadInvoice.CopyToAsync(fileStream);
            //}

            //string fileUrl = GenerateFileUrl(fileName);

            // Save the photo data and ID to the database
            var photo = new InvoiceImage { InvoiceId = invoiceId, Image = photoData };
            var isInvoiceImageAdded = await _unitOfWork.GetRepository<InvoiceImage>().Add(photo);
            if (!isInvoiceImageAdded)
            {
                throw new ApplicationException();
            }
            await _unitOfWork.Save();
            var response = new ApiResponse<dynamic>
            {
                StatusCode = (int)StatusCode.Success,
                Success = true,
                Data = new { Id = photo.InvoiceId }
            };
            return response;
        }
        private string GenerateFileUrl(string filePath)
        {
            // Convert file path to a URI
            Uri fileUri = new Uri(filePath);

            // Get the absolute URL
            string fileUrl = fileUri.AbsoluteUri;

            return fileUrl;
        }
    }
}
