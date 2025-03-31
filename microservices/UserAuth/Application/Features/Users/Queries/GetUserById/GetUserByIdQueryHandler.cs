using BuildingBlocks.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async Task<GetUserByIdResponse> Handle(
    GetUserByIdQuery request,
    CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id)
        ?? throw new NotFoundException($"User with ID {request.Id} not found");

        return new GetUserByIdResponse
        {
            Email = user.Email!
        };
    }
}