
using MediatR;

namespace Application.Features.Users.Commands.LogoutUser;

public class LogoutUserCommand: IRequest<LogoutUserResponse>
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
