
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationAdmin : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    //public string? RefreshToken { get; set; }
    //public DateTime RefreshTokenExpiryTime { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
