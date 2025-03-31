
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetByTrackerAsync(string tracker)
    {
        return await _dbSet
            .FirstOrDefaultAsync(o => o.Tracker == tracker);
    }

    public async Task UpdateOrderStatusAsync(int orderId,OrderStatus status)
     => await _dbSet
         .Where(o => o.Id == orderId)
         .ExecuteUpdateAsync(s => s
             .SetProperty(o => o.OrderStatus, status));
}