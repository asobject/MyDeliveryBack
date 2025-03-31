using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T?> GetByIdAsync(int id);
        //Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids);
        //Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        //Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        //Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<PagedResultDTO<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate = null,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs);

        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs);
        Task<IEnumerable<T>> FindBySqlAsync(
         string sqlQuery,
         params object[] parameters);

    }
}
