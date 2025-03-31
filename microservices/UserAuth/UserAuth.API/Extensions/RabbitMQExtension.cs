using BuildingBlocks.Messaging.Extensions;
using Infrastructure.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserAuth.API.Extensions;

internal static class RabbitMQExtension
{
    internal static void ConfigureRabbit(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddRabbitMqBus(
            configuration,
            cfg =>
            {
                cfg.AddConsumer<OrderCreatedConsumer>();
            });
    }
}
