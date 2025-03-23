

namespace Application.Interfaces.Services;

public interface IRefreshTokenStore
{
    Task StoreTokenAsync(string userId, string jti, string refreshToken);
    Task<bool> UpdateTokenAsync(
       string userId,
       string oldJti,
       string newJti,
       string oldRefreshToken,
       string newRefreshToken);
    Task RevokeTokenAsync(string userId, string jti);
    Task<string?> GetTokenAsync(string userId, string jti);
    Task<bool> TokenExistsAsync(string userId, string jti);

}