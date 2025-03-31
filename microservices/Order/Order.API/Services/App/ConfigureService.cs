
using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using Application.Features.Orders.Commands.CreateOrder;
using Domain.Entities;
using Order.API.Extensions;

namespace Order.API.Services.App
{
    public static class ConfigureService
    {
        public static void ConfigureHosting(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpContextAccessor();

            services.ConfigureCors(configuration);
            services.ConfigureContextNpgsql(configuration);
            //services.ConfigureRedis(configuration);
            services.ConfigureRabbit(configuration);
            services.AddMediatR(cfg =>
         cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

            services.AddScoped<IAppConfiguration, AppConfiguration>();
            services.AddScoped<IRepository<DeliveryPoint>, Repository<DeliveryPoint>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderRepository, OrderRepository>();


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
