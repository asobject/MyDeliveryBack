

using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;

namespace Application.Features.Users.Commands.RefreshTokenUser;

public class RefreshTokenUserCommandHandler(
    ITokenExtractionService tokenExtractionService,
    ITokenService tokenService,
    IUserRepository userRepository,
    IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<RefreshTokenUserCommand, RefreshTokenUserResponse>
{
    public async Task<RefreshTokenUserResponse> Handle(
        RefreshTokenUserCommand request,
        CancellationToken cancellationToken)
    {
        var oldAccessTokenData = tokenService.GetData(request.AccessToken);

        var user = await userRepository.GetByIdAsync(oldAccessTokenData.Sub)
            ?? throw new NotFoundException("User not found");

        var storedToken = await refreshTokenStore.GetTokenAsync(user.Id, oldAccessTokenData.Jti);

        if (storedToken !=  request.RefreshToken)
        {
            await refreshTokenStore.RevokeTokenAsync(user.Id, oldAccessTokenData.Jti);
            throw new InvalidTokenException("Invalid refresh token");
        }

        var userRoles = await userRepository.GetRolesAsync(user);
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

        return new RefreshTokenUserResponse
        {
            AccessToken = newAccessToken,
            Message = "Refresh successful"
        };
    }
}
