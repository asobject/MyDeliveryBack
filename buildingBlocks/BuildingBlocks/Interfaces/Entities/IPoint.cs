

using BuildingBlocks.Models;

namespace BuildingBlocks.Interfaces.Entities;

public interface IPoint
{
    string Address { get; set; }
    GeoPoint Coordinates { get; set; }
}
