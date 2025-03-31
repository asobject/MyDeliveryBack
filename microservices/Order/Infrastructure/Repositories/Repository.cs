using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : class, IEntity
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);
    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<PagedResultDTO<T>> GetPagedAsync(
     int pageNumber,
     int pageSize,
     Expression<Func<T, bool>>? predicate = null,
     params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs)
    {
        // Начинаем с базового запроса
        IQueryable<T> baseQuery = _dbSet.AsQueryable();

        // Применяем фильтр, если он задан
        if (predicate != null)
        {
            baseQuery = baseQuery.Where(predicate);
        }

        // Вычисляем общее количество записей по базовому запросу
        int totalRecords = await baseQuery.CountAsync();

        // Применяем функции инклюдов для загрузки связанных сущностей
        IQueryable<T> query = baseQuery;
        foreach (var include in includeFuncs)
        {
            query = include(query);
        }

        // Добавляем сортировку по полю "Id"
        query = query.OrderBy(e => EF.Property<object>(e, "Id"));

        // Применяем пагинацию
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Возвращаем результат в виде PagedResultDTO<T>
        return new PagedResultDTO<T>
        {
            Data = data,
            TotalRecords = totalRecords
        };
    }


    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    => await _dbSet.AnyAsync(predicate);

    public async Task<IEnumerable<T>> FindAsync(
     Expression<Func<T, bool>> predicate,
     params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs)
    {
        IQueryable<T> query = _dbSet.Where(predicate);

        foreach (var include in includeFuncs)
        {
            query = include(query);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindBySqlAsync(
    string sqlQuery,
    params object[] parameters)
    {
        return await _dbSet.FromSqlRaw(sqlQuery, parameters).ToListAsync();
    }
}