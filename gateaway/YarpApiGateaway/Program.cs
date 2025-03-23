
using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using YarpApiGateaway.Extensions;

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

            builder.Services.ConfigureAuthentication(builder.Configuration);



            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:4200",
                            "http://angular-admin",
                            "http://angular-client",
                            "http://angular-worker")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });



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
            builder.Services.AddTransient<GlobalExceptionMiddleware>();

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.MapReverseProxy();
            app.Run();
        }
    }
}
