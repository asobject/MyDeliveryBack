namespace BuildingBlocks.Interfaces.Repositories;

public interface ITransaction : IDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}
