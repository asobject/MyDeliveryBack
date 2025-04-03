
using Application.Features.Orders.Commands.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        _ = await mediator.Send(command);
        return Created();
    }
}
