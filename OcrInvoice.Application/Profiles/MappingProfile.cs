using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OcrInvoice.Application.Models;
using OcrInvoice.Domain.Models;

namespace OcrInvoice.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<InvoiceMaster, Worksheet1Item>().ReverseMap().
                ForMember(dest => dest.ProviderName, act => act.MapFrom(src => src.SenderName))
               .ForMember(dest => dest.Address, act => act.MapFrom(src => src.SenderAddress))
               .ForMember(dest => dest.City, act => act.MapFrom(src => src.SenderCity))
               .ForMember(dest => dest.Country, act => act.MapFrom(src => src.SenderCountry))
               .ForMember(dest => dest.ZipCode, act => act.MapFrom(src => src.SenderZipCode))
               .ForMember(dest => dest.Date, act => act.MapFrom(src => src.InvoiceDate))
               .ForMember(dest => dest.TaxId, act => act.MapFrom(src => src.SenderGSTNumber))
               .ForMember(dest => dest.Total, act => act.MapFrom(src => src.TotalAmount));

            CreateMap<Customer, Worksheet1Item>().ReverseMap()
               .ForMember(dest => dest.Name, act => act.MapFrom(src => src.ReceiverName))
               .ForMember(dest => dest.Address, act => act.MapFrom(src => src.ReceiverAddress))
               .ForMember(dest => dest.TaxId, act => act.MapFrom(src => src.ReceiverGSTNumber));

            CreateMap<InvoiceResponse, InvoiceMaster>()
                .ForMember(dest =>
                   dest.InvoiceID,
                   opt => opt.MapFrom(src => src.Id))
               .ForMember(dest =>
                   dest.Date,
                   opt => opt.MapFrom(src => src.ExpenseDate))
               .ForMember(dest =>
                   dest.Title,
                   opt => opt.MapFrom(src => src.Title))
               .ForMember(dest =>
                   dest.InvoiceNumber,
                   opt => opt.MapFrom(src => src.BillNo))
               .ForMember(dest =>
                   dest.Total,
                   opt => opt.MapFrom(src => src.TotalAmount))
               .ForMember(dest =>
                   dest.TotalTaxes,
                   opt => opt.MapFrom(src => src.TotalTaxes))
               .ForMember(dest =>
                   dest.Comments,
                   opt => opt.MapFrom(src => src.Comments))
               .ForMember(dest =>
                   dest.ProjectName,
                   opt => opt.MapFrom(src => src.ProjectName))
               .ForMember(dest =>
                   dest.OcrPercentage,
                   opt => opt.MapFrom(src => src.OcrPercentage))
               .ForMember(dest =>
                   dest.Status,
                   opt => opt.MapFrom(src => src.Status))
               .ForMember(dest =>
                   dest.ProviderName,
                   opt => opt.MapFrom(src => src.MerchantName))
               .ForMember(dest =>
                   dest.InvoiceCategory,
                   opt => opt.MapFrom(src => src.CategoryName))
               .ForMember(dest =>
                   dest.Comments,
                   opt => opt.MapFrom(src => src.Comments))
               .ForMember(dest =>
                   dest.UpdatedBy,
                   opt => opt.MapFrom(src => src.UpdatedBy))
               .ReverseMap();

            CreateMap<LineItemMaster, Product>()
                .ForMember(dest =>
                   dest.ProductName,
                   opt => opt.MapFrom(src => src.ItemName))
               .ForMember(dest =>
                   dest.ProductAmount,
                   opt => opt.MapFrom(src => src.Price))
               .ReverseMap();
        }
    }
}
