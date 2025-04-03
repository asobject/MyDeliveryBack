using MediatR;

namespace Application.Features.Admin.Commands.LoginAdmin;

public class LoginAdminCommand : IRequest<LoginAdminResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
