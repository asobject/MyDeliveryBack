using Infrastructure.Services;
using Application.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;
using Application.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using Admin.API.Extensions;
using Application.Features.Admin.Commands.RegisterAdmin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;

namespace Admin.API.Services.App
{
    public static class ConfigureService
    {
        public static void ConfigureHosting(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpContextAccessor();

            services.ConfigureCors(configuration);
            services.ConfigureContextNpgsql(configuration);
            services.ConfigureRedis(configuration);
            services.ConfigureSwagger();
            services.ConfigureIdentity();
            services.ConfigureRabbit(configuration);
            //var assembly = typeof(Program).Assembly;
            //services.AddMediatR(config =>
            //{
            //    config.RegisterServicesFromAssembly(assembly);
            //    config.AddOpenBehavior(typeof(GetUserByIdQuery));
            //});
            services.AddMediatR(cfg =>
         cfg.RegisterServicesFromAssembly(typeof(RegisterAdminCommand).Assembly));

            services.AddScoped<IAppConfiguration, AppConfiguration>();
            services.AddScoped<IRefreshTokenStore, RedisRefreshTokenStore>();


            services.AddScoped<IAdminRepository, AdminRepository>();


            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenExtractionService, TokenExtractionService>();

            services.AddControllers();

            services.AddLogging();
           

            services.AddTransient<GlobalExceptionMiddleware>();

            services.Configure<RouteOptions>(options => {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

      
        }
    }
}
