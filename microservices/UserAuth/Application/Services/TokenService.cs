

using Application.DTOs;
using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;

public class TokenService(IAppConfiguration configuration) : ITokenService
{
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public JwtSecurityToken GenerateAccessToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue("JWT_USER_SECRET")));
        _ = int.TryParse(configuration.GetValue("JWT_USER_ACCESS_TOKEN_VALIDITY"), out int tokenValidityInMinutes);

        var token = new JwtSecurityToken(
            issuer: configuration.GetValue("JWT_ISSUER"),
            audience: configuration.GetValue("JWT_AUDIENCE"),
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        return token;
    }

    public ClaimsPrincipal ValidateToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue("JWT_USER_SECRET"))),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException($"Invalid {nameof(accessToken)}");

        return principal;
    }
    public TokenDataDTO GetData(string accessToken)
    {
        try
        {
            var principal = ValidateToken(accessToken);
            var claims = principal.Claims;

            string GetClaimValue(params string[] types)
            {
                foreach (var type in types)
                {
                    var claim = claims.FirstOrDefault(c => c.Type == type);
                    if (claim != null) return claim.Value;
                }
                throw new InvalidTokenException($"Missing required claims: {string.Join(", ", types)}");
            }

            var expValue = GetClaimValue(ClaimTypes.Expired, "exp");
            if (!long.TryParse(expValue, out var unixTime))
                throw new InvalidTokenException("Invalid exp format");

            return new TokenDataDTO
            {
                Email = GetClaimValue(ClaimTypes.Email,"email"),
                Sub = GetClaimValue(ClaimTypes.NameIdentifier,"sub"),
                Jti = GetClaimValue("jti"),
                FirstName = GetClaimValue("firstName"),
                Roles = [.. claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "roles").Select(c => c.Value)],
                Exp = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime
            };
        }
        catch (SecurityTokenException ex)
        {
            throw new InvalidTokenException($"Token validation failed:{ex}");
        }
    }
    public (string AccessToken, string RefreshToken, DateTime RefreshTokenExpiryTime) GenerateTokens(ApplicationUser user, IList<Claim> additionalClaims)
    {
        var claims = new List<Claim>
    {
        new ("sub",user.Id),
        new("email", user.Email!),
        new ("firstName",user.FirstName),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

        claims.AddRange(additionalClaims);

        var token = GenerateAccessToken(claims);
        var refreshToken = GenerateRefreshToken();

        var refreshTokenExpiryTime = DateTime.Now.AddDays(
            int.TryParse(configuration.GetValue("JWT_USER_REFRESH_TOKEN_VALIDITY"), out var days) ? days : 7
        );
        return (new JwtSecurityTokenHandler().WriteToken(token), refreshToken, refreshTokenExpiryTime);
    }
}