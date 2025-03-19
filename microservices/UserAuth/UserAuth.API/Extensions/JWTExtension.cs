using Application.Exceptions;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.API.Extensions;

internal static class JWTExtension
{
    internal static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => ConfigureJwtOptions(options, configuration, validateLifetime: true))
.AddJwtBearer("AllowExpiredToken", options => ConfigureJwtOptions(options, configuration, validateLifetime: false, "AllowExpiredToken"));
    }
    private static void ConfigureJwtOptions(JwtBearerOptions options, IConfiguration configuration, bool validateLifetime, string schemePrefix = null)
    {
        // Общие настройки для всех схем
        options.TokenValidationParameters = CreateTokenValidationParameters(configuration, validateLifetime);
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        // Настройка событий с учетом префикса схемы
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context => HandleAuthenticationFailed(context, schemePrefix),
            OnTokenValidated = context => HandleTokenValidated(context)
        };
    }

    private static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration, bool validateLifetime)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)),
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

    private static async Task HandleTokenValidated(TokenValidatedContext context)
    {
        try
        {
            if (context.Principal == null)
                throw new InvalidTokenException("Invalid token: principal not found");

            var userId = context.Principal.FindFirstValue("id");

            if (string.IsNullOrEmpty(userId))
                throw new InvalidTokenException("Invalid token: user ID claim missing");

            var userRepository = context.HttpContext.RequestServices
                .GetRequiredService<IUserRepository>();

            var user = await userRepository.GetByIdAsync(userId) ??
                throw new NotFoundException("User not found");
        }
        catch (Exception ex)
        {
            context.Fail(ex); // Пробрасываем исключение в контекст
            throw; // Пробрасываем дальше для глобального обработчика
        }
    }
}
