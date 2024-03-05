using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OcrInvoice.Application.Contracts.Respository;
using OcrInvoice.Application.Services;

namespace OcrInvoice.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IInvoiceCreateRepository, InvoiceCreateRepository>();
  
            return services;
        }
    }
}
