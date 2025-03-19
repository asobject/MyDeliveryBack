

using MediatR;

namespace Application.Features.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<LoginUserResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
