using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationAdmin>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityRole>().HasData(
           new IdentityRole { Id = "1", Name = Domain.Constants.AdminRoles.SuperAdmin, NormalizedName = Domain.Constants.AdminRoles.SuperAdmin.ToUpper() },
           new IdentityRole { Id = "2", Name = Domain.Constants.AdminRoles.Admin, NormalizedName = Domain.Constants.AdminRoles.Admin.ToUpper() }
       );

        var adminId = Guid.NewGuid().ToString();
        var admin = new ApplicationAdmin
        {
            Id = adminId,
            UserName = "admin@example.com",
            NormalizedUserName = "ADMIN@EXAMPLE.COM",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = new PasswordHasher<ApplicationAdmin>().HashPassword(null, "admin@example.com"),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        modelBuilder.Entity<ApplicationAdmin>().HasData(admin);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
             new IdentityUserRole<string>
             {
                 RoleId = "1",
                 UserId = adminId
             },
            new IdentityUserRole<string>
            {
                RoleId = "2",
                UserId = adminId
            }
        );
    }
}
