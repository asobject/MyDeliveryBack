

using MediatR;

namespace Application.Features.Users.Commands.RefreshTokenUser;

public class RefreshTokenUserCommand : IRequest<RefreshTokenUserResponse>
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
