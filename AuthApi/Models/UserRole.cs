using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApi.Models;

public class UserRole
{
    [Key]
    public int Id { get; set; }


    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

   
    [Required]
    public int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }
}
