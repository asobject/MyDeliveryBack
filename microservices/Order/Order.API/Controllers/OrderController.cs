
using Application.Features.Orders.Commands.CreateOrder;
using BuildingBlocks.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(IMediator mediator, ITokenExtractionService tokenExtractionService) : ControllerBase
{
    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        _ = await mediator.Send(command);
        return Created();
    }
}
