
using Application.Features.Orders.Queries.CalculateOrder;

namespace Application.Interfaces;

public interface ICalculationService
{
    double CalculatePrice(CalculateOrderQuery order);
}