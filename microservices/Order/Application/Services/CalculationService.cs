
using Application.Features.Orders.Queries.CalculateOrder;
using Application.Interfaces;
using BuildingBlocks.Models;
using Domain.Enums;
namespace Application.Services;

public class CalculationService : ICalculationService
{
    private const double EarthRadius = 6371.0;

    private double CalculateDistance(GeoPoint startPoint, GeoPoint endPoint)
    {
        // Преобразуем координаты в радианы
        var startLatRad = DegreesToRadians(startPoint.Latitude);
        var startLonRad = DegreesToRadians(startPoint.Longitude);
        var endLatRad = DegreesToRadians(endPoint.Latitude);
        var endLonRad = DegreesToRadians(endPoint.Longitude);

        // Разница между долготой и широтой
        var deltaLat = endLatRad - startLatRad;
        var deltaLon = endLonRad - startLonRad;

        // Формула Haversine
        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(startLatRad) * Math.Cos(endLatRad) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Возвращаем расстояние в километрах
        return EarthRadius * c;
    }

    // Метод для преобразования градусов в радианы
    private double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180.0);
    }

    public double CalculatePrice(CalculateOrderQuery order)
    {

        var x = CalculateDistance(order.StartPointCoordinates, order.EndPointCoordinates);
        if (x < 500)
            x = 500;
        if (order.ReceiverDeliveryMethod == DeliveryMethod.CourierCall)
            x += 250;
        if (order.SenderDeliveryMethod == DeliveryMethod.CourierCall)
            x += 250;
        if (order.PaymentType == PaymentType.Receiver)
            x *= 1.1;
        return Math.Round(x, 2);
    }
}
