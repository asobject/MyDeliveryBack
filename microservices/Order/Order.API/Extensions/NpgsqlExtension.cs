using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Order.API.Extensions;

internal static class NpgsqlExtension
{
    internal static void ConfigureContextNpgsql(this IServiceCollection services, ConfigurationManager configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString,
             o => o.UseNetTopologySuite()
                 .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
