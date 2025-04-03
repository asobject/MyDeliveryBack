using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Point = NetTopologySuite.Geometries.Point;
using BuildingBlocks.Models;

namespace Infrastructure.Services;

public class GeoPointConverter : ValueConverter<GeoPoint, Point>
{
    public GeoPointConverter() : base(
        domainValue => domainValue.ToProvider(),
        dbValue => dbValue.FromProvider())
    {
    }
}

public static class GeoPointExtensions
{
    private static readonly GeometryFactory _geometryFactory = new(new PrecisionModel(), 4326);

    public static Point ToProvider(this GeoPoint point)
    {
        return _geometryFactory.CreatePoint(new Coordinate(point.Longitude, point.Latitude));
    }
    public static GeoPoint? FromProvider(this Point dbPoint)
    {
        if (dbPoint == null || dbPoint.IsEmpty)
            return null;

        return new GeoPoint(
            dbPoint.Coordinate.Y,  // Latitude
            dbPoint.Coordinate.X   // Longitude
        );
    }
}