

using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

public class DeliveryPoint : IEntity
{
    public int Id { get; set; }
    public DeliveryMethod Method { get; set; }
    public string? Address { get; set; }
    public int? CompanyPointId { get; set; }
    public ICollection<Order> SenderOrders { get; set; } = [];

    public ICollection<Order> ReceiverOrders { get; set; } = [];
}
