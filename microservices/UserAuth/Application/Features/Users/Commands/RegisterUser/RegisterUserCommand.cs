

using MediatR;

namespace Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
    public string FirstName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
