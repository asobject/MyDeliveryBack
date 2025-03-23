
using BuildingBlocks.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace YarpApiGateaway.Extensions;

internal static class JWTExtension
{
    internal static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer("UserAuth", options => ConfigureJwtOptions(options, configuration, validateLifetime: false, "USER"))
.AddJwtBearer("User", options => ConfigureJwtOptions(options, configuration, validateLifetime: true, "USER"));

        services.AddAuthorizationBuilder()
            .AddPolicy("UserAuthPolicy", policy =>
            {
                policy.AuthenticationSchemes.Add("UserAuth");
                policy.RequireAuthenticatedUser();
            })
            .AddPolicy("UserPolicy", policy =>
            {
                policy.AuthenticationSchemes.Add("User");
                policy.RequireAuthenticatedUser();
            });
    }
    private static void ConfigureJwtOptions(JwtBearerOptions options, IConfiguration configuration, bool validateLifetime, string schemePrefix)
    {
        // Общие настройки для всех схем
        options.TokenValidationParameters = CreateTokenValidationParameters(configuration, validateLifetime,schemePrefix);
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        // Настройка событий с учетом префикса схемы
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context => HandleAuthenticationFailed(context, schemePrefix),
            OnTokenValidated = context => HandleTokenValidated(context)
        };
    }

    private static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration, bool validateLifetime, string schemePrefix)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT_AUDIENCE"],
            ValidIssuer = configuration["JWT_ISSUER"],
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[$"JWT_{schemePrefix}_SECRET"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    }

    private static Task HandleAuthenticationFailed(AuthenticationFailedContext context, string schemePrefix)
    {
        var logMessage = string.IsNullOrEmpty(schemePrefix)
            ? $"Authentication failed: {context.Exception.Message}"
            : $"[{schemePrefix}] Authentication failed: {context.Exception.Message}";

        Console.WriteLine(logMessage);
        return Task.CompletedTask;
    }

    private static Task HandleTokenValidated(TokenValidatedContext context)
    {
        try
        {
            //var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
            //var authHeader = context.HttpContext.Request.Headers.Authorization.ToString();
            //if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            //    throw new InvalidTokenException("Missing or invalid authorization header.");

            //var accessToken = authHeader["Bearer ".Length..].Trim();

            // Используем TokenService для получения данных из токена (внутри происходит валидация и проверка необходимых клеймов)
           // var tokenData = tokenService.GetData(accessToken);
            //if (context.Principal == null)
            //    throw new InvalidTokenException("Invalid token: principal not found");

            //var userId = context.Principal.FindFirstValue("id");

            //if (string.IsNullOrEmpty(userId))
            //    throw new InvalidTokenException("Invalid token: user ID claim missing");

            //var userRepository = context.HttpContext.RequestServices
            //    .GetRequiredService<IUserRepository>();

            //var user = await userRepository.GetByIdAsync(userId) ??
            //    throw new NotFoundException("User not found");
        }
        catch (Exception ex)
        {
            context.Fail(ex); // Пробрасываем исключение в контекст
            //throw; // Пробрасываем дальше для глобального обработчика
        }
        return Task.CompletedTask;
    }
}
