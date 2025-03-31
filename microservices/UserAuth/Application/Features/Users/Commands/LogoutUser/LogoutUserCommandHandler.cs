

using Application.Interfaces.Services;
using BuildingBlocks.Interfaces.Services;
using MediatR;

namespace Application.Features.Users.Commands.LogoutUser;

public class LogoutUserCommandHandler(
    ITokenExtractionService tokenExtractionService,
    ITokenService tokenService,
    IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<LogoutUserCommand, LogoutUserResponse>
{
    public async Task<LogoutUserResponse> Handle(
        LogoutUserCommand request,
        CancellationToken cancellationToken)
    {
        var tokenData = tokenService.GetData(request.AccessToken);
        await refreshTokenStore.RevokeTokenAsync(tokenData.Sub, tokenData.Jti);
        tokenExtractionService.RemoveRefreshTokenFromCookie();
        return new LogoutUserResponse
        {
            Message = "Logout successful"
        };
    }
}
