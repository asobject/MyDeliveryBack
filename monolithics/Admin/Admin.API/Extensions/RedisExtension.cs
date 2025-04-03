
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Admin.API.Extensions;

internal static class RedisExtension
{
    internal static void ConfigureRedis(this IServiceCollection services, ConfigurationManager configuration)
    {
        string connectionString = configuration.GetConnectionString("Redis");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "AuthCache_";
        });
        services.AddSingleton<IConnectionMultiplexer>(_ =>
       ConnectionMultiplexer.Connect(connectionString));
    }
}
