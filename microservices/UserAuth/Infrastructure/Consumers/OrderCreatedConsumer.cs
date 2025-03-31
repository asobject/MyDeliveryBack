

using BuildingBlocks.Messaging.Events;
using Domain.Interfaces.Repositories;
using MassTransit;

namespace Infrastructure.Consumers;

public class OrderCreatedConsumer(
    IUserRepository userRepository,
    IPublishEndpoint publishEndpoint)
    : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var user = await userRepository.GetByEmailAsync(context.Message.ReceiverEmail);

            await publishEndpoint.Publish(new UserVerifiedEvent(
                OrderId: context.Message.OrderId,
                ReceiverUserId: user?.Id
            ));
 
    }
}
