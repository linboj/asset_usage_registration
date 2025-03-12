using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

/// <summary>
/// Salt model, representing a salt used in user password hashing and salting.
/// </summary>
public class Salt: BaseEntity
{
    /// <summary>
    /// The name of salt
    /// </summary>
    [Column("value")]
    public string Value { get; set; } = string.Empty;


    /// <summary>
    /// The associated user.
    /// </summary>
    [Required]
    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    public User User { get; set; }  = null!;
}