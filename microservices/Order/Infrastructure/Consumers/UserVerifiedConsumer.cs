

using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using MassTransit;

namespace Infrastructure.Consumers;

public class UserVerifiedConsumer(
    IUnitOfWork unitOfWork)
    : IConsumer<UserVerifiedEvent>
{
    //private readonly IOrderRepository _orderRepository = (IOrderRepository)unitOfWork.Orders;
    public async Task Consume(ConsumeContext<UserVerifiedEvent> context)
    {
        var order = await unitOfWork.Orders
            .GetByIdAsync(context.Message.OrderId) ?? throw new NotFoundException($"Order:{context.Message.OrderId} not found");
        order.ReceiverId = context.Message.ReceiverUserId;
        await unitOfWork.CompleteAsync();
    }
}