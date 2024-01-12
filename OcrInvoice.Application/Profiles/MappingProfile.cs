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
        }

        public double ConvertStringToDouble(string input)
        {
            double result;

            // Try to parse the string to a double
            if (Double.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}
