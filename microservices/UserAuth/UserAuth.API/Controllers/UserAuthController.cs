
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.LogoutUser;
using Application.Features.Users.Commands.RefreshTokenUser;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Queries.GetUserById;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UserAuth.API.Controllers;

[ApiController]
[Route("api/user-auth")]
public class UserAuthController(IMediator mediator, ITokenExtractionService tokenExtractionService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var response = await mediator.Send(command);

        return CreatedAtAction(nameof(GetUserById), new { id = response.UserId }, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var response = await mediator.Send(command);

        return Ok(response);
    }
    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        GetUserByIdQuery query = new()
        {
            UserId = id
        };
        var user = await mediator.Send(query);

        return user == null
            ? NotFound()
            : Ok(user);
    }
    [HttpPut("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        RefreshTokenUserCommand command = new()
        {
            AccessToken = tokenExtractionService.GetAccessTokenFromHeader(),
            RefreshToken = tokenExtractionService.GetRefreshTokenFromCookie()
        };
        var response = await mediator.Send(command);
        return Ok(response);
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        LogoutUserCommand command = new()
        {
            AccessToken = tokenExtractionService.GetAccessTokenFromHeader(),
            RefreshToken = tokenExtractionService.GetRefreshTokenFromCookie()
        };
        _ = await mediator.Send(command);
        return NoContent();
    }
}