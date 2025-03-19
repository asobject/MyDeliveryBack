

using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class TokenDataDTO
{
    [Required(ErrorMessage = "Sub is required")]
    public string Sub { get; set; } = null!;
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = null!;
    [Required(ErrorMessage = "Jti is required")]
    public string Jti { get; set; } = null!;
    [Required(ErrorMessage = "Roles is required")]
    public List<string> Roles { get; set; } = null!;
    [Required(ErrorMessage = "Exp is required")]
    public DateTime Exp { get; set; }
}
