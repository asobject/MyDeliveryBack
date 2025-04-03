

using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IAdminRepository
{
    Task<ApplicationAdmin?> GetByIdAsync(string id);
    Task<ApplicationAdmin?> GetByEmailAsync(string email);
    Task<ApplicationAdmin?> GetByUsernameAsync(string username);
    Task<IEnumerable<ApplicationAdmin>> GetAllAsync();
    Task<bool> CreateAsync(ApplicationAdmin user, string password);
    Task UpdateAsync(ApplicationAdmin user);
    Task DeleteAsync(ApplicationAdmin user);
    Task AddToRoleAsync(ApplicationAdmin user, string role);
    Task<IList<string>> GetRolesAsync(ApplicationAdmin user);
    Task<bool> CheckPasswordAsync(ApplicationAdmin user, string password);
    Task<bool> ExistsAsync(string id);
    Task<bool> RoleExistsAsync(string roleName);
}