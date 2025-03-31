
using MediatR;

namespace Application.Features.Users.Queries.GetUserByEmail;

public class GetUserByEmailQuery : IRequest<GetUserByEmailResponse>
{
    public string Email { get; set; } = null!;
}
