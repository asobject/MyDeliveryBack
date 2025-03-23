using Infrastructure.Services;
using UserAuth.API.Extensions;
using Application.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Application.Features.Users.Queries.GetUserById;
using BuildingBlocks.Exceptions;

namespace UserAuth.API.Services.App
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
            //var assembly = typeof(Program).Assembly;
            //services.AddMediatR(config =>
            //{
            //    config.RegisterServicesFromAssembly(assembly);
            //    config.AddOpenBehavior(typeof(GetUserByIdQuery));
            //});
            services.AddMediatR(cfg =>
         cfg.RegisterServicesFromAssembly(typeof(GetUserByIdQuery).Assembly));

            services.AddScoped<IAppConfiguration, AppConfiguration>();
           // services.AddScoped<IAuthorizationFilter, AuthorizeTokenValidationFilter>();
            services.AddScoped<IRefreshTokenStore, RedisRefreshTokenStore>();


            services.AddScoped<IUserRepository, UserRepository>();


            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenExtractionService, TokenExtractionService>();



            services.AddLogging();
            services.AddControllers(options =>
            {
              //  options.Filters.Add<AuthorizeTokenValidationFilter>(); 
            });

            services.AddTransient<GlobalExceptionMiddleware>();

            services.Configure<RouteOptions>(options => {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

      
        }
    }
}
