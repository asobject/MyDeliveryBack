

namespace Application.Interfaces.Services;

public interface ITokenExtractionService
{
    string GetAccessTokenFromHeader();
    string GetRefreshTokenFromCookie();
    void SetRefreshTokenInCookie(string refreshToken);
    void RemoveRefreshTokenFromCookie();
}