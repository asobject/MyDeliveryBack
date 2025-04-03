using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;

namespace Application.Features.Admin.Commands.RefreshTokenAdmin;

public class RefreshTokenAdminCommandHandler(
    ITokenExtractionService tokenExtractionService,
    ITokenService tokenService,
    IAdminRepository adminRepository,
    IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<RefreshTokenAdminCommand, RefreshTokenAdminResponse>
{
    public async Task<RefreshTokenAdminResponse> Handle(
        RefreshTokenAdminCommand request,
        CancellationToken cancellationToken)
    {
        var oldAccessTokenData = tokenService.GetData(request.AccessToken);

        var user = await adminRepository.GetByIdAsync(oldAccessTokenData.Sub)
            ?? throw new NotFoundException("User not found");

        var storedToken = await refreshTokenStore.GetTokenAsync(user.Id, oldAccessTokenData.Jti);

        if (storedToken !=  request.RefreshToken)
        {
            await refreshTokenStore.RevokeTokenAsync(user.Id, oldAccessTokenData.Jti);
            throw new InvalidTokenException("Invalid refresh token");
        }

        var userRoles = await adminRepository.GetRolesAsync(user);
        var claims = userRoles.Select(role => new Claim("roles", role)).ToList();

        var (newAccessToken, newRefreshToken, refreshTokenExpiryTime) =
            tokenService.GenerateTokens(user, claims);
        var newAccessTokenData = tokenService.GetData(newAccessToken);

        await refreshTokenStore.UpdateTokenAsync(
            user.Id,
            oldAccessTokenData.Jti,
            newAccessTokenData.Jti,
            request.RefreshToken,
            newRefreshToken);

        tokenExtractionService.SetRefreshTokenInCookie(newRefreshToken);

        return new RefreshTokenAdminResponse
        {
            AccessToken = newAccessToken,
            Message = "Refresh successful"
        };
    }
}
