using MediatR;

namespace Application.Features.Admin.Commands.RefreshTokenAdmin;

public class RefreshTokenAdminCommand : IRequest<RefreshTokenAdminResponse>
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
