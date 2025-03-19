using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace UserAuth.API.Extensions;

internal static class NpgsqlExtension
{
    internal static void ConfigureContextNpgsql(this IServiceCollection services, ConfigurationManager configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
//.Replace("{DB_HOST}", configuration["DB_HOST"])
//.Replace("{DB_PORT}", configuration["DB_PORT"])
//.Replace("{DB_USER}", configuration["DB_USER"])
//.Replace("{DB_PASSWORD}", configuration["DB_PASSWORD"]);
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
