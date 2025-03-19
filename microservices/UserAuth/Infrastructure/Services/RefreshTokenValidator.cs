
using Application.Exceptions;
using Application.Interfaces.Services;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Services;

public class RefreshTokenValidator(ITokenExtractionService tokenExtractionService, ITokenService tokenService,IRefreshTokenStore refreshTokenStore)
{
    //public async Task<bool> RefreshTokensEqual()
    //{
    //    var access = tokenExtractionService.GetAccessTokenFromHeader();
    //    var data = tokenService.GetData(access);
    //    var refreshTokenFromCookie = tokenExtractionService.GetRefreshTokenFromCookie();
    //    return await refreshTokenStore.ValidateTokenAsync(data.Sub, refreshTokenFromCookie);
    //}
}
