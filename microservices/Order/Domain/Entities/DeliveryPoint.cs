

using BuildingBlocks.Interfaces.Entities;
using Domain.Enums;

namespace Domain.Entities;

public class DeliveryPoint : IEntity
{
    public int Id { get; set; }
    public DeliveryMethod Method { get; set; }
    public int? CustomPointId { get; set; }
    public CustomPoint? CustomPoint { get; set; }
    public int? CompanyPointId { get; set; }
    public ICollection<Order> SenderOrders { get; set; } = [];

    public ICollection<Order> ReceiverOrders { get; set; } = [];
}
