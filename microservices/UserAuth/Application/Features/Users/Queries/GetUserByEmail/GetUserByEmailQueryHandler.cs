

using BuildingBlocks.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByEmailQuery, GetUserByEmailResponse>
{
    public async Task<GetUserByEmailResponse> Handle(
    GetUserByEmailQuery request,
    CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email)
        ?? throw new NotFoundException($"User with ID {request.Email} not found");

        return new GetUserByEmailResponse
        {
            Id=user.Id
        };
    }
}