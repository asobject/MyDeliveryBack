using BuildingBlocks.Models;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Queries.CalculateOrder;

public class CalculateOrderQuery : IRequest<CalculateOrderResponse>
{
    public GeoPoint StartPointCoordinates { get; set; } = null!;
    public GeoPoint EndPointCoordinates { get; set; } = null!;
    public DeliveryMethod SenderDeliveryMethod { get; set; }
    public DeliveryMethod ReceiverDeliveryMethod { get; set; }
    public PaymentType PaymentType { get; set; }
}
