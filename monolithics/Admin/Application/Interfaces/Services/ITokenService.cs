

using Application.DTOs;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Interfaces.Services;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(List<Claim> authClaims);
    ClaimsPrincipal ValidateToken(string token);
    string GenerateRefreshToken();
    TokenDataDTO GetData(string accessToken);
    (string AccessToken, string RefreshToken, DateTime RefreshTokenExpiryTime) GenerateTokens(ApplicationAdmin admin, IList<Claim> additionalClaims);
}
