
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace YarpApiGateaway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
     .AddEnvironmentVariables();

            builder.Services.AddReverseProxy()
       .ConfigureHttpClient((context, handler) =>
       {
           if (handler is SocketsHttpHandler socketsHandler)
           {
               socketsHandler.SslOptions.RemoteCertificateValidationCallback =
                   (sender, certificate, chain, sslPolicyErrors) => true;
           }
       })
       .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));



            var app = builder.Build();
            app.UseHttpsRedirection();
            app.MapReverseProxy();
            app.Run();
        }
    }
}
