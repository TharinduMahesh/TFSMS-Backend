using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Services;
using test6API.Hubs;
using System.ComponentModel.DataAnnotations;


var builder = WebApplication.CreateBuilder(args);

// --- Configure Services ---

// 1. Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add Email Service
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddScoped<IEmailService, SendGridEmailService>();

// 3. Add SignalR for Chat Functionality
builder.Services.AddSignalR();

// 4. Add Controllers
builder.Services.AddControllers();

// 5. Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("FlutterAppPolicy", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 6. Add API Explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// --- Build the App ---
var app = builder.Build();


// --- Configure the HTTP Request Pipeline ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("FlutterAppPolicy");

app.UseAuthorization();

app.MapControllers();

// Map the SignalR Hub to the "/chathub" endpoint
app.MapHub<ChatHub>("/chathub");


// --- Run the App ---
app.Run();
