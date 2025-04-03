using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;

namespace Application.Features.Admin.Commands.LoginAdmin;

public class LoginAdminCommandHandler(IAdminRepository adminRepository, ITokenService tokenService, ITokenExtractionService tokenExtractionService,IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<LoginAdminCommand, LoginAdminResponse>
{
    public async Task<LoginAdminResponse> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            throw new ConflictException("Invalid email format");

        var user = await adminRepository.GetByEmailAsync(request.Email)
            ?? throw new NotFoundException("User not found");


        var isPasswordValid = await adminRepository.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new PasswordIncorrectException("Invalid password");

        var userRoles = await adminRepository.GetRolesAsync(user);

        var claims = userRoles.Select(role => new Claim("roles", role)).ToList();
        (string accessToken, string refreshToken, DateTime refreshTokenExpiryTime) = tokenService.GenerateTokens(user, claims);
        var data = tokenService.GetData(accessToken);

        await refreshTokenStore.StoreTokenAsync(user.Id,data.Jti, refreshToken);

        tokenExtractionService.SetRefreshTokenInCookie(refreshToken);
        //user.RefreshToken = refreshToken;
        //user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

        //await userRepository.UpdateAsync(user);

        return new LoginAdminResponse
        {
            AccessToken = accessToken,
            Message = "Login successful"
        };
    }
    private static bool IsValidEmail(string email)
    {
        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA0-9.-]+\.[a-zA-Z]{2,}$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
    }
}
