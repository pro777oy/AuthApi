using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models;

public class Role
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    // 🔗 Navigation: one role can belong to many users
    public ICollection<UserRole>? UserRoles { get; set; }
}
