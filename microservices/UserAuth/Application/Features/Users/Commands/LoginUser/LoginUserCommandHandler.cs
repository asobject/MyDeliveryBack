

using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Security.Claims;

namespace Application.Features.Users.Commands.LoginUser;

public class LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService, ITokenExtractionService tokenExtractionService,IRefreshTokenStore refreshTokenStore)
    : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            throw new ConflictException("Invalid email format");

        var user = await userRepository.GetByEmailAsync(request.Email)
            ?? throw new NotFoundException("User not found");


        var isPasswordValid = await userRepository.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new PasswordIncorrectException("Invalid password");

        var userRoles = await userRepository.GetRolesAsync(user);

        var claims = userRoles.Select(role => new Claim("roles", role)).ToList();
        (string accessToken, string refreshToken, DateTime refreshTokenExpiryTime) = tokenService.GenerateTokens(user, claims);
        var data = tokenService.GetData(accessToken);

        await refreshTokenStore.StoreTokenAsync(user.Id,data.Jti, refreshToken);

        tokenExtractionService.SetRefreshTokenInCookie(refreshToken);
        //user.RefreshToken = refreshToken;
        //user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

        //await userRepository.UpdateAsync(user);

        return new LoginUserResponse
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
