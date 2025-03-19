

using Application.Exceptions;
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
        var user = await userRepository.GetByIdAsync(request.UserId)
        ?? throw new NotFoundException($"User with ID {request.UserId} not found");

        return new GetUserByIdResponse
        {
            FirstName = user.FirstName,
            Email = user.Email!
        };
    }
}