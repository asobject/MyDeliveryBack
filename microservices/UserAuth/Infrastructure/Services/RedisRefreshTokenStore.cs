

using Application.Exceptions;
using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisRefreshTokenStore(IConnectionMultiplexer redis, IConfiguration config) : IRefreshTokenStore
{
    private readonly IDatabase _redis = redis.GetDatabase();
    private readonly TimeSpan _tokenLifetime = TimeSpan.FromDays(config.GetValue<int>("JWT_REFRESH_TOKEN_VALIDITY"));

    public async Task StoreTokenAsync(string userId, string jti, string refreshToken)
    {
        var key = $"refresh_tokens:{userId}:{jti}";
        await _redis.StringSetAsync(key, refreshToken, _tokenLifetime);
    }

    public async Task<bool> UpdateTokenAsync(
       string userId,
       string oldJti,
       string newJti,
       string oldRefreshToken,
       string newRefreshToken)
    {
        var storedToken = await _redis.StringGetAsync($"refresh_tokens:{userId}:{oldJti}");
        if (storedToken != oldRefreshToken)
        {
            throw new InvalidTokenException("Invalid refresh token");
        }

        await _redis.KeyDeleteAsync($"refresh_tokens:{userId}:{oldJti}");

        await StoreTokenAsync(userId, newJti, newRefreshToken);
        return true;
    }
    public async Task RevokeTokenAsync(string userId, string jti)
    {
        if (!await _redis.KeyDeleteAsync($"refresh_tokens:{userId}:{jti}"))
        {
            throw new InvalidTokenException("Token not found for revocation");
        }
    }
    public async Task<string?> GetTokenAsync(string userId, string jti)
    {
        return await _redis.StringGetAsync($"refresh_tokens:{userId}:{jti}");
    }
}