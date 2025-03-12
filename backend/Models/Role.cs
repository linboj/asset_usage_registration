using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

/// <summary>
/// Role model, representing a role of users in the system.
/// </summary>
public class Role: BaseEntity
{
    /// <summary>
    /// The name of the role.
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the associated user.
    /// </summary>
    [Required]
    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    /// The associated user.
    /// </summary>
    public User User { get; set; }  = null!;
}