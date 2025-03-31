

using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<CreateOrderResponse>
{
    public string SenderId { get; set; } = null!;
    public string ReceiverEmail { get; set; } = null!;
    public decimal Price { get; set; }
    public PaymentType PaymentType { get; set; }
    public PackageSize PackageSize { get; set; }
    public DeliveryMethod SenderDeliveryMethod { get; set; }
    public string? SenderAddress { get; set; }
    public int? SenderCompanyPointId { get; set; }
    public DeliveryMethod ReceiverDeliveryMethod { get; set; }
    public string? ReceiverAddress { get; set; }
    public int? ReceiverCompanyPointId { get; set; }
}
