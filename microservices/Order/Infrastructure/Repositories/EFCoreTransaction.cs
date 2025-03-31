
using BuildingBlocks.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories;

public class EFCoreTransaction(IDbContextTransaction transaction) : ITransaction
{
    public async Task CommitAsync()
    {
        await transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await transaction.RollbackAsync();
    }

    public void Dispose()
    {
        transaction.Dispose();
    }
}