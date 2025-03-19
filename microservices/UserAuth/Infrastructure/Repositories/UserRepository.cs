using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IUserRepository
{
    public async Task<ApplicationUser?> GetByIdAsync(string id)
        => await userManager.FindByIdAsync(id);

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
        => await userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        => await userManager.FindByNameAsync(username);

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        => await userManager.Users.ToListAsync();

    public async Task<bool> ExistsAsync(string id)
        => await userManager.Users.AnyAsync(u => u.Id == id);

    public async Task<bool> CreateAsync(ApplicationUser user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        await this.AddToRoleAsync(user, UserRoles.User);
        return result.Succeeded;
    }
    public async Task<bool> RoleExistsAsync(string roleName)
           => await roleManager.RoleExistsAsync(roleName);

    public async Task UpdateAsync(ApplicationUser user)
        => await userManager.UpdateAsync(user);

    public async Task DeleteAsync(ApplicationUser user)
        => await userManager.DeleteAsync(user);

    public async Task AddToRoleAsync(ApplicationUser user, string role)
        => await userManager.AddToRoleAsync(user, role);

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        => await userManager.GetRolesAsync(user);

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        => await userManager.CheckPasswordAsync(user, password);
}