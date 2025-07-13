using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Other service configurations...

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Configure JSON serialization to use camelCase
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        // Your other service configurations...
    }

    // Rest of your Startup class...
}
