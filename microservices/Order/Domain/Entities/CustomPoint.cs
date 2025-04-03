

using BuildingBlocks.Interfaces.Entities;
using BuildingBlocks.Models;

namespace Domain.Entities;

public class CustomPoint : IEntity,IPoint
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public GeoPoint Coordinates { get; set; } = null!; // Использует NetTopologySuite.Geometries.Point
    public ICollection<DeliveryPoint> DeliveryPoints { get; set; } = [];
}
