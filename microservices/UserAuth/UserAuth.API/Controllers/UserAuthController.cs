
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.RegisterUser;
using Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UserAuth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAuthController(IMediator mediator) : ControllerBase
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
    //[Authorize(AuthenticationSchemes = "AllowExpiredToken")]
    //[HttpPut("refresh")]
    //public async Task<IActionResult> RefreshToken()
    //{
    //    var response = await authService.RefreshTokenAsync();
    //    return Ok(response); 
    //}
    //[Authorize(AuthenticationSchemes = "AllowExpiredToken")]
    //[HttpPost("logout")]
    //public async Task<IActionResult> Logout()
    //{
    //    await authService.SignOutAsync();
    //    return NoContent();
    //}
}