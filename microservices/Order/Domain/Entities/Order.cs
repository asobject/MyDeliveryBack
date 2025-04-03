

using BuildingBlocks.Interfaces.Entities;
using Domain.Enums;

namespace Domain.Entities;

public class Order : IEntity
{
    public int Id { get; set; }
    public string Tracker { get; set; } = Guid.NewGuid().ToString();
    public decimal Price { get; set; }
    public PaymentType PaymentType { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Created;
    public int? CurrentPointId { get; set; }
    public PackageSize PackageSize { get; set; }

    // Отправитель (обязательно зарегистрирован)
    public string SenderId { get; set; } = null!;

    // Получатель (может быть незарегистрированным)
    public string? ReceiverId { get; set; }
    public string ReceiverEmail { get; set; } = null!;

    public int SenderDeliveryPointId { get; set; }
    public DeliveryPoint SenderDeliveryPoint { get; set; } = null!;
    public int ReceiverDeliveryPointId { get; set; }
    public DeliveryPoint ReceiverDeliveryPoint { get; set; } = null!;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveryDate { get; set; } = null;
}