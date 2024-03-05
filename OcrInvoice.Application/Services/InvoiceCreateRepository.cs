using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvoiceCreateRepository(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<dynamic>> CreateAndUploadInvoice(InvoiceRequest invoice)
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();

            //if (jwtToken == null) {
            //    throw new ApplicationException("Token Authentication Failed");
            //}

            //var user = ExtractDataFromToken(jwtToken);

            InvoiceStatus status = InvoiceStatus.Pending;
            

            var invoiceMasterData = new InvoiceMaster
            {
                Date = invoice.ExpenseDate,
                Title = invoice.Title,
                ProviderName = invoice.MerchantName,
                InvoiceNumber = invoice.BillNo,
                Total = ConvertToDoubleOrDefault(invoice.TotalAmount),
                OcrPercentage = invoice.OcrPercentage,
                InvoiceCategory = invoice.CategoryName,
                ProjectName = invoice.ProjectName,
                TotalTaxes = ConvertToDoubleOrDefault(invoice.TotalTaxes),
                ImageUrl = invoice.ImageUrl,
                Status = status.ToString(),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            var isInvoiceMasterAdded = await _unitOfWork.GetRepository<InvoiceMaster>().Add(invoiceMasterData);
            if (!isInvoiceMasterAdded)
            {
                throw new ApplicationException("Invoice not added");
            }
            await _unitOfWork.Save();
            var lineItems = new List<LineItemMaster>();
            if (invoice.Products != null && invoice.Products.Count > 0)
            { 
                foreach (var item in invoice.Products) {
                    lineItems.Add(new LineItemMaster
                    {
                        InvoiceID = invoiceMasterData.InvoiceID,
                        ItemName = item.ProductName,
                        Price = ConvertToDoubleOrDefault(item.ProductAmount)
                    });
                }
            }
            var isLineItemsAdded = await _unitOfWork.GetRepository<LineItemMaster>().AddRange(lineItems);
            if (!isLineItemsAdded)
            {
                throw new ApplicationException("Line Items or Product not added");
            }
            await _unitOfWork.Save();
            var response = new ApiResponse<dynamic> {
                StatusCode = 200,
                Success = true,
                Data = "Your Bill is Successfully Submitted"
            };
            return response;
        }

        public static double ConvertToDoubleOrDefault(string input)
        {
            if (double.TryParse(input, out double result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }


        public dynamic ExtractDataFromToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;

            dynamic tokenData = new System.Dynamic.ExpandoObject();

            if (jwtSecurityToken != null)
            {
                foreach (Claim claim in jwtSecurityToken.Claims)
                {
                    ((IDictionary<string, object>)tokenData)[claim.Type] = claim.Value;
                }
            }

            return tokenData;
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

        public async Task<ApiResponse<dynamic>> GetAllInvoices()
        {
            var invoiceMasterData = await _unitOfWork.GetRepository<InvoiceMaster>().GetAll();
            invoiceMasterData = invoiceMasterData.OrderByDescending(x => x.UpdatedDate).ToList();
            var responseData = new List<InvoiceResponse>();

            foreach (var item in invoiceMasterData)
            {
                responseData.Add(new InvoiceResponse { 
                    Id = item.InvoiceID,
                    ImageUrl = item.ImageUrl,
                    ExpenseDate = item.Date,
                    Status = item.Status,
                    BillNo = item.InvoiceNumber,
                    CategoryName = item.InvoiceCategory,
                    Comments = item.Comments,
                    MerchantName = item.ProviderName,
                    ProjectName = item.ProjectName,
                    Title = item.Title,
                    TotalAmount = item.Total.ToString(),
                    TotalTaxes = item.TotalTaxes.ToString(),
                    OcrPercentage = item.OcrPercentage
                });
            }

            var response = new ApiResponse<dynamic> { 
                StatusCode = 200,
                Success = true,
                Data = responseData
            };

            return response;
        }

        public async Task<ApiResponse<dynamic>> UploadInvoice(IFormFile invoiceFile)
        {
            if (invoiceFile == null || invoiceFile.Length == 0)
            {
                throw new ApplicationException("No file uploaded.");
            }

            // Read the file data into a byte array
            byte[] photoData;
            using (var memoryStream = new MemoryStream())
            {
                await invoiceFile.CopyToAsync(memoryStream);
                photoData = memoryStream.ToArray();
            }

            var currentdirectory = Directory.GetCurrentDirectory().ToString();
            var directoryPath = Path.Combine(currentdirectory, "Images");

            // Check if the directory exists, and create it if it doesn't
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(invoiceFile.FileName);
            var fileExtension = Path.GetExtension(invoiceFile.FileName);
            var dateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{fileNameWithoutExtension}_{dateTimeString}{fileExtension}";
            var path = Path.Combine(directoryPath, fileName);

            // Use FileStream to directly save the file
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await invoiceFile.CopyToAsync(fileStream);
            }

            string fileUrl = string.Concat("Images","/",fileName);

            var response = new ApiResponse<dynamic>
            {
                StatusCode = (int)StatusCode.Success,
                Success = true,
                Data = fileUrl
            };
            return response;
        }

        public string GetCurrentUrl(HttpContext context)
        {
            var request = context.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            // Combine the base URL, path, and query string (if any) to get the complete URL
            var fullUrl = $"{baseUrl}";

            return fullUrl;
        }


        public async Task<ApiResponse<dynamic>> DeleteInvoice(int invoiceId)
        {
            var invoiceData = await _unitOfWork.GetRepository<InvoiceMaster>().GetById(invoiceId);
            if (invoiceData == null)
            {
                throw new ApplicationException("invoice data not found");
            }
            var isDeleted = await _unitOfWork.GetRepository<InvoiceMaster>().Delete(invoiceId);
            if (!isDeleted)
            {
                throw new ApplicationException("Data not deleted");
            }
            await _unitOfWork.Save();
            var response = new ApiResponse<dynamic>
            {
                StatusCode = 200,
                Success = true,
                Data = "Invoice data deleted successfully"
            };
            return response;
        }

        public async Task<ApiResponse<dynamic>> GetInvoiceImageByInvoiceId(int invoiceId)
        {
            var invoiceData = await _unitOfWork.GetRepository<InvoiceMaster>().GetById(invoiceId);

            if (invoiceData == null)
            {
                throw new ApplicationException("Invoice data not found");
            }

            var lineItems = await _unitOfWork.GetRepository<LineItemMaster>().GetAll();
            var lineItemsData = lineItems.Where(x => x.InvoiceID == invoiceData.InvoiceID).ToList();
            var serializedData = _mapper.Map<List<Product>>(lineItemsData);
            var data = _mapper.Map<InvoiceResponse>(invoiceData);
            data.Products = serializedData;
            var response = new ApiResponse<dynamic>
            {
                StatusCode = 200,
                Success = true,
                Data = data
            };
            return response;
        }

        public async Task<ApiResponse<dynamic>> EditInvoiceData(InvoiceResponse invoice)
        {
            var invoiceData = await _unitOfWork.GetRepository<InvoiceMaster>().GetById(invoice.Id);

            _mapper.Map(invoice, invoiceData);
            invoiceData.UpdatedDate = DateTime.UtcNow;
            var isUpdated = await _unitOfWork.GetRepository<InvoiceMaster>().Upsert(invoiceData);
            if (!isUpdated)
            {
                throw new ApplicationException("not updated");
            }
            await _unitOfWork.Save();
            var response = new ApiResponse<dynamic>
            {
                StatusCode = 200,
                Success = true,
                Data = "Invoice Updated Successfully"
            };
            return response;
        }

        public async Task<ApiResponse<dynamic>> UpdateStatusById(int invoiceId, string status)
        {
            var invoiceData = await _unitOfWork.GetRepository<InvoiceMaster>().GetById(invoiceId);

            if (string.IsNullOrEmpty(status))
            {
                throw new ApplicationException("Invalid Status");
            }

            if (invoiceData == null )
            {
                throw new ApplicationException("Invalid Invoice id");
            }

            invoiceData.Status = status;
            invoiceData.UpdatedDate = DateTime.UtcNow;
            var isUpdated = await _unitOfWork.GetRepository<InvoiceMaster>().Upsert(invoiceData);
            if (!isUpdated)
            {
                throw new ApplicationException("not updated");
            }
            await _unitOfWork.Save();
            var response = new ApiResponse<dynamic>
            {
                StatusCode = 200,
                Success = true,
                Data = "status updated successfully"
            };
            return response;

        }
    }
}
