
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Order.API.Services.App;

namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddEnvironmentVariables();

            builder.Services.ConfigureHosting(builder.Configuration);


            var app = builder.Build();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Migration")
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
                return;
            }
            app.ConfigurePipeline();

            app.Run();
        }
    }
}
