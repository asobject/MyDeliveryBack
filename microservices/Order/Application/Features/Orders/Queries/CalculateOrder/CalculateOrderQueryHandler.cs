using MediatR;

namespace Application.Features.Orders.Queries.CalculateOrder;

public class CalculateOrderQueryHandler() : IRequestHandler<CalculateOrderQuery, CalculateOrderResponse>
{
    public async Task<CalculateOrderResponse> Handle(CalculateOrderQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new CalculateOrderResponse
        {
            Price = 0
        });
    }
}
