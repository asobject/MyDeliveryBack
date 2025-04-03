

using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    //private readonly IOrderRepository _orderRepository = (IOrderRepository)unitOfWork.Orders;
    //private readonly IRepository<DeliveryPoint> _pointRepository = (IRepository<DeliveryPoint>)unitOfWork.DeliveryPoints;
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.SenderId))
            throw new ConflictException("SenderId is required.");

        if (string.IsNullOrWhiteSpace(request.ReceiverEmail))
            throw new ConflictException("Receiver email is required.");

        if (!new EmailAddressAttribute().IsValid(request.ReceiverEmail))
            throw new ConflictException("Invalid receiver email format.");

        if (request.Price <= 0)
            throw new ConflictException("Price must be greater than zero.");

        if (!Enum.IsDefined(typeof(PaymentType), request.PaymentType))
            throw new ConflictException("Invalid payment type.");

        if (!Enum.IsDefined(typeof(PackageSize), request.PackageSize))
            throw new ConflictException("Invalid package size.");

        ValidateDeliveryMethod(
            request.SenderDeliveryMethod,
            request.SenderCompanyPointId,
            request.SenderAddress,
            "sender"
        );

        ValidateDeliveryMethod(
            request.ReceiverDeliveryMethod,
            request.ReceiverCompanyPointId,
            request.ReceiverAddress,
            "receiver"
        );


        using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var senderPoint = await GetOrCreateDeliveryPoint(
                request.SenderDeliveryMethod,
                request.SenderCompanyPointId,
                request.SenderAddress
            );

            var receiverPoint = await GetOrCreateDeliveryPoint(
                request.ReceiverDeliveryMethod,
                request.ReceiverCompanyPointId,
                request.ReceiverAddress
            );

            var order = new Order
            {
                Price = request.Price,
                PaymentType = request.PaymentType,
                SenderId = request.SenderId,
                PackageSize = request.PackageSize,
                SenderDeliveryPointId = senderPoint.Id,
                ReceiverDeliveryPointId = receiverPoint.Id
            };

            await unitOfWork.Orders.AddAsync(order);
            await unitOfWork.CompleteAsync();
            await transaction.CommitAsync();

            await publishEndpoint.Publish(new OrderCreatedEvent(
               OrderId: order.Id,
               ReceiverEmail: request.ReceiverEmail
           ), cancellationToken);

            return new CreateOrderResponse
            {
                Tracker = order.Tracker,
                Message = "Order created successfully"
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    private static void ValidateDeliveryMethod(
    DeliveryMethod method,
    int? companyPointId,
    string? address,
    string target)
    {
        if (!Enum.IsDefined(typeof(DeliveryMethod), method))
            throw new ConflictException($"Invalid {target} delivery method.");

        if (method == DeliveryMethod.PickupPoint && !companyPointId.HasValue)
            throw new ConflictException($"CompanyPointId is required for {target}'s PickupPoint delivery.");

        if (method == DeliveryMethod.CourierCall && string.IsNullOrWhiteSpace(address))
            throw new ConflictException($"Address is required for {target}'s CourierCall delivery.");
    }
    private async Task<DeliveryPoint> GetOrCreateDeliveryPoint(
     DeliveryMethod method,
     int? companyPointId,
     string? address)
    {
        DeliveryPoint? existingPoint = null;

        if (method == DeliveryMethod.PickupPoint)
        {
            // Для пункта выдачи ищем по companyPointId
            existingPoint = (await unitOfWork.DeliveryPoints.FindAsync(p =>
                p.Method == method &&
                p.CompanyPointId == companyPointId))
                .FirstOrDefault();
        }
        else if (method == DeliveryMethod.CourierCall)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentException("Address required for courier delivery");

            // Находим или создаем кастомный пункт
            var customPoint = await GetOrCreateCustomPoint(address);

            // Ищем точку доставки с привязкой к кастомному пункту
            existingPoint = (await unitOfWork.DeliveryPoints.FindAsync(p =>
                p.Method == method &&
                p.CustomPointId == customPoint.Id))
                .FirstOrDefault();
        }

        if (existingPoint != null)
            return existingPoint;

        // Создаем новую точку доставки
        var newPoint = new DeliveryPoint
        {
            Method = method,
            CompanyPointId = method == DeliveryMethod.PickupPoint ? companyPointId : null,
            CustomPointId = method == DeliveryMethod.CourierCall
                ? (await GetOrCreateCustomPoint(address!)).Id
                : null
        };

        await unitOfWork.DeliveryPoints.AddAsync(newPoint);
        await unitOfWork.CompleteAsync();
        return newPoint;
    }

    private async Task<CustomPoint> GetOrCreateCustomPoint(string address)
    {
        var existing = (await unitOfWork.CustomPoints.FindAsync(c =>
            c.Address == address))
            .FirstOrDefault();

        if (existing != null)
            return existing;

        var newPoint = new CustomPoint { Address = address };
        await unitOfWork.CustomPoints.AddAsync(newPoint);
        await unitOfWork.CompleteAsync();
        return newPoint;
    }
}
