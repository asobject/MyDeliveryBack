using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdminRepository(UserManager<ApplicationAdmin> userManager, RoleManager<IdentityRole> roleManager) : IAdminRepository
{
    public async Task<ApplicationAdmin?> GetByIdAsync(string id)
        => await userManager.FindByIdAsync(id);

    public async Task<ApplicationAdmin?> GetByEmailAsync(string email)
        => await userManager.FindByEmailAsync(email);

    public async Task<ApplicationAdmin?> GetByUsernameAsync(string username)
        => await userManager.FindByNameAsync(username);

    public async Task<IEnumerable<ApplicationAdmin>> GetAllAsync()
        => await userManager.Users.ToListAsync();

    public async Task<bool> ExistsAsync(string id)
        => await userManager.Users.AnyAsync(u => u.Id == id);

    public async Task<bool> CreateAsync(ApplicationAdmin admin, string password)
    {
        var result = await userManager.CreateAsync(admin, password);
        await this.AddToRoleAsync(admin, AdminRoles.Admin);
        return result.Succeeded;
    }
    public async Task<bool> RoleExistsAsync(string roleName)
           => await roleManager.RoleExistsAsync(roleName);

    public async Task UpdateAsync(ApplicationAdmin admin)
        => await userManager.UpdateAsync(admin);

    public async Task DeleteAsync(ApplicationAdmin admin)
        => await userManager.DeleteAsync(admin);

    public async Task AddToRoleAsync(ApplicationAdmin admin, string role)
        => await userManager.AddToRoleAsync(admin, role);

    public async Task<IList<string>> GetRolesAsync(ApplicationAdmin admin)
        => await userManager.GetRolesAsync(admin);

    public async Task<bool> CheckPasswordAsync(ApplicationAdmin admin, string password)
        => await userManager.CheckPasswordAsync(admin, password);
}