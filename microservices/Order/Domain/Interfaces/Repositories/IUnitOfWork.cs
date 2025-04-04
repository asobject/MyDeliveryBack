﻿
using BuildingBlocks.Interfaces.Repositories;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<Order> Orders { get; }
    IRepository<DeliveryPoint> DeliveryPoints { get; }
    IRepository<CustomPoint> CustomPoints { get; }
    Task<int> CompleteAsync();
    Task<ITransaction> BeginTransactionAsync();
}