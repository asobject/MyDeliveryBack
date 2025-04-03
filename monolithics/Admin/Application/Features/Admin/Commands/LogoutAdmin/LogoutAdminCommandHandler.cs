using Application.Interfaces.Services;
using BuildingBlocks.Interfaces.Services;
using MediatR;

namespace Application.Features.Admin.Commands.LogoutAdmin;

public class LogoutAdminCommandHandler(
    ITokenExtractionService tokenExtractionService,
    ITokenService tokenService,
    IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<LogoutAdminCommand, LogoutAdminResponse>
{
    public async Task<LogoutAdminResponse> Handle(
        LogoutAdminCommand request,
        CancellationToken cancellationToken)
    {
        var tokenData = tokenService.GetData(request.AccessToken);
        await refreshTokenStore.RevokeTokenAsync(tokenData.Sub, tokenData.Jti);
        tokenExtractionService.RemoveRefreshTokenFromCookie();
        return new LogoutAdminResponse
        {
            Message = "Logout successful"
        };
    }
}
