

namespace Application.Features.Users.Commands.RefreshTokenUser;

public class RefreshTokenUserResponse
{
    public string AccessToken { get; set; } = null!;
    public string Message { get; set; } = null!;
}
