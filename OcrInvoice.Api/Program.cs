using Microsoft.Extensions.FileProviders;
using OcrInvoice.Api.Middleware;
using OcrInvoice.Application;
using OcrInvoice.Infrastructure;
using OcrInvoice.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.AddDirectoryBrowser();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

//Configure CORS Policy for Application
builder.Services.AddCors(options =>
{
    options.AddPolicy("Ocr-CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
 
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

//Add Cors Policy in Application
app.UseCors("Ocr-CorsPolicy");

var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images"));
var requestPath = "/Images";

// Enable displaying browser links.
app.UseFileServer(new FileServerOptions
{
    FileProvider = fileProvider,
    RequestPath = requestPath
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = fileProvider,
    RequestPath = requestPath,
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();