using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 🔗 Navigation: one user can have multiple roles
    public ICollection<UserRole>? UserRoles { get; set; }
}
