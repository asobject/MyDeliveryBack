using Application.Features.Admin.Commands.LoginAdmin;
using Application.Features.Admin.Commands.LogoutAdmin;
using Application.Features.Admin.Commands.RefreshTokenAdmin;
using Application.Features.Admin.Commands.RegisterAdmin;
using BuildingBlocks.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Admin.API.Controllers;

[ApiController]
[Route("api/admin-auth")]
public class AdminAuthController(IMediator mediator, ITokenExtractionService tokenExtractionService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAdminCommand command)
    {
        _ = await mediator.Send(command);
        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginAdminCommand command)
    {
        var response = await mediator.Send(command);

        return Ok(response);
    }
    [HttpPut("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        RefreshTokenAdminCommand command = new()
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
        LogoutAdminCommand command = new()
        {
            AccessToken = tokenExtractionService.GetAccessTokenFromHeader(),
            RefreshToken = tokenExtractionService.GetRefreshTokenFromCookie()
        };
        _ = await mediator.Send(command);
        return NoContent();
    }
}