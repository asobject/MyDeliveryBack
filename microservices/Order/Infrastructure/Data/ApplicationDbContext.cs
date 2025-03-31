

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<DeliveryPoint> DeliveryPoints { get; set; } = null!;
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
    }
}
