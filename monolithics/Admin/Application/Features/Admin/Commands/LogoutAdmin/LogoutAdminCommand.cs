using MediatR;

namespace Application.Features.Admin.Commands.LogoutAdmin;

public class LogoutAdminCommand: IRequest<LogoutAdminResponse>
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
