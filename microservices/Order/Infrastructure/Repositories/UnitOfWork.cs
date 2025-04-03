
using Infrastructure.Data;
using Domain.Interfaces.Repositories;
using BuildingBlocks.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        public IRepository<Order> Orders => new  OrderRepository(context);
        public IRepository<DeliveryPoint> DeliveryPoints => new Repository<DeliveryPoint>(context);
        public IRepository<CustomPoint> CustomPoints => new Repository<CustomPoint>(context);
        public async Task<int> CompleteAsync() => await context.SaveChangesAsync();
        public async Task<ITransaction> BeginTransactionAsync()
        {
            var dbTransaction = await context.Database.BeginTransactionAsync();
            return new EFCoreTransaction(dbTransaction);
        }
        public void Dispose() => context.Dispose();
    }
}
