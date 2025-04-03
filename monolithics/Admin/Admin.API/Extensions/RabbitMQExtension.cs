using BuildingBlocks.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.API.Extensions;

internal static class RabbitMQExtension
{
    internal static void ConfigureRabbit(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddRabbitMqBus(
            configuration,
            cfg =>
            {
            });
    }
}
