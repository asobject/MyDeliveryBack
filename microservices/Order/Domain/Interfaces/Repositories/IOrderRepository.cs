

using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.Repositories;

public interface IOrderRepository:IRepository<Order>
{
    Task<Order?> GetByTrackerAsync(string tracker);
    Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
}
