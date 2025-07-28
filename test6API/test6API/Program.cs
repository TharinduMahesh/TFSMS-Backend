using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Services; // <-- Make sure this is imported

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- ADD THESE LINES FOR EMAIL VERIFICATION ---
// Binds the "SendGrid" section from appsettings.json to the SendGridSettings class
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
// Registers the SendGridEmailService for dependency injection
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
// --- END OF ADDED LINES ---

// Your existing services (if any)
// builder.Services.AddScoped<ICollectorService, CollectorService>();
// builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICollectorService, CollectorService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
