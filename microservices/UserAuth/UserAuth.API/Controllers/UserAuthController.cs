
using Application.Features.Users.Commands.LoginUser;
using Application.Features.Users.Commands.LogoutUser;
using Application.Features.Users.Commands.RefreshTokenUser;
using Application.Features.Users.Commands.RegisterUser;
using BuildingBlocks.Interfaces.Services;
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
        _ = await mediator.Send(command);
        return Created();
        //return CreatedAtAction(nameof(GetUserByEmail), new { email = response.Email }, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var response = await mediator.Send(command);

        return Ok(response);
    }
    //[HttpGet("users")]
    //public async Task<IActionResult> GetUserById([FromQuery] string id)
    //{
    //    GetUserByIdQuery query = new()
    //    {
    //        Id = id
    //    };
    //    var user = await mediator.Send(query);

    //    return Ok(user);
    //}
    //[HttpGet("users")]
    //public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    //{
    //    GetUserByEmailQuery query = new()
    //    {
    //        Email = email
    //    };
    //    var user = await mediator.Send(query);

    //    return Ok(user);
    //}
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