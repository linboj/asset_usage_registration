using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

/// <summary>
/// Role model, representing a role of users in the borrowing system.
/// </summary>
public class Role: BaseEntity
{
    /// <summary>
    /// The name of role
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = string.Empty;


    /// <summary>
    /// The associated user.
    /// </summary>
    [Required]
    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    public User User { get; set; }  = null!;
}