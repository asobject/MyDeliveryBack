

using Domain.Entities;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<DeliveryPoint> DeliveryPoints { get; set; } = null!;
    public DbSet<CustomPoint> CustomPoints { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Связь: DeliveryPoint (отправитель) -> Order
        modelBuilder.Entity<DeliveryPoint>()
            .HasMany(dp => dp.SenderOrders) // Коллекция заказов-отправителей
            .WithOne(o => o.SenderDeliveryPoint) // Навигационное свойство в Order
            .HasForeignKey(o => o.SenderDeliveryPointId)
            .OnDelete(DeleteBehavior.Restrict); // Чтобы избежать каскадного удаления

        // Связь: DeliveryPoint (получатель) -> Order
        modelBuilder.Entity<DeliveryPoint>()
            .HasMany(dp => dp.ReceiverOrders) // Коллекция заказов-получателей
            .WithOne(o => o.ReceiverDeliveryPoint) // Навигационное свойство в Order
            .HasForeignKey(o => o.ReceiverDeliveryPointId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomPoint>(entity =>
        {

            // Преобразование GeoPoint через конвертер
            entity.Property(p => p.Coordinates)
                .HasConversion<GeoPointConverter>()
              .HasColumnType("geography(Point, 4326)");
            // Добавление индекса на Coordinates
            entity.HasIndex(p => p.Coordinates)
                .HasMethod("GIST");  // Используем GIST для географического индекса

            entity.HasMany(cp => cp.DeliveryPoints)
                .WithOne(o => o.CustomPoint)
                .HasForeignKey(o => o.CustomPointId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
