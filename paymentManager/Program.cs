

//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;
//using paymentManager.Data;
//using paymentManager.Services;
//using System;
//using System.Text.Json.Serialization;


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//        options.JsonSerializerOptions.PropertyNamingPolicy = null;
//        options.JsonSerializerOptions.WriteIndented = true;
//    });

//// Add DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        sqlOptions => sqlOptions.EnableRetryOnFailure(
//            maxRetryCount: 5,
//            maxRetryDelay: TimeSpan.FromSeconds(30),
//            errorNumbersToAdd: null)
//    ));

//// Register services
//builder.Services.AddScoped<ISupplierService, SupplierService>();
//builder.Services.AddScoped<IGreenLeafService, GreenLeafService>();
//builder.Services.AddScoped<IPaymentService, PaymentService>();
//builder.Services.AddScoped<IAdvanceService, AdvanceService>();
//builder.Services.AddScoped<IDebtService, DebtService>();
//builder.Services.AddScoped<IIncentiveService, IncentiveService>();
//builder.Services.AddScoped<IReceiptService, ReceiptService>();
//builder.Services.AddScoped<IExportService, ExportService>();

//// Add CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngularApp",
//        builder => builder
//            .WithOrigins("http://localhost:4200") // Angular app URL
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials());
//});

//// Add Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tea Payment API", Version = "v1" });
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tea Payment API v1"));
//}

//app.UseHttpsRedirection();

//app.UseCors("AllowAngularApp");

//app.UseAuthorization();

//app.MapControllers();

//// Apply migrations automatically in development
//if (app.Environment.IsDevelopment())
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        dbContext.Database.Migrate();
//    }
//}

//app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using paymentManager.Data;
using paymentManager.Services;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    ));

// Register services
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<GreenLeafService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAdvanceService, AdvanceService>();
builder.Services.AddScoped<IDebtService, DebtService>();
builder.Services.AddScoped<IIncentiveService, IncentiveService>();

builder.Services.AddScoped<IDenaturedTeaService, DenaturedTeaService>();
builder.Services.AddScoped<ITeaReturnService, TeaReturnService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
// Add this line to your Program.cs where you register other services

// Add CORS with more permissive settings for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .AllowAnyOrigin() // More permissive for development
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tea Payment API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tea Payment API v1"));
}

// In development, you might want to disable HTTPS redirection temporarily
// to avoid SSL certificate issues
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Use CORS before routing
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();



app.Run();