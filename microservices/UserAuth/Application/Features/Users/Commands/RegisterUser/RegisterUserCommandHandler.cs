using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
        {
            throw new ConflictException("Invalid email format");
        }

        var existingUser = await userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new AlreadyExistsException("User with this email already exists");
        }

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            UserName=request.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = request.Email
        };

        await userRepository.CreateAsync(user, request.Password);


        return new RegisterUserResponse
        {
            UserId = user.Id,
            Message = "User registered successfully"
        };
    }
    private static bool IsValidEmail(string email)
    {
        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA0-9.-]+\.[a-zA-Z]{2,}$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
    }
}

