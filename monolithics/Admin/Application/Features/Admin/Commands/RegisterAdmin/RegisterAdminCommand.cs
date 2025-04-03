using MediatR;

namespace Application.Features.Admin.Commands.RegisterAdmin;

public class RegisterAdminCommand : IRequest<RegisterAdminResponse>
{
    public string FirstName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
