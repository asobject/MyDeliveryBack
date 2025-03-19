

using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task<ApplicationUser?> GetByUsernameAsync(string username);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<bool> CreateAsync(ApplicationUser user, string password);
    Task UpdateAsync(ApplicationUser user);
    Task DeleteAsync(ApplicationUser user);
    Task AddToRoleAsync(ApplicationUser user, string role);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<bool> ExistsAsync(string id);
    Task<bool> RoleExistsAsync(string roleName);
}