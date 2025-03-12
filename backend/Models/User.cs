using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

/// <summary>
/// User model, representing a user in the borrowing system.
/// </summary>
[Index(nameof(UserName), IsUnique = true)]
public class User : BaseEntity
{
    /// <summary>
    /// The user's login username.
    /// </summary>
    [Column("user_name")]
    public required string UserName { get; set; }

    /// <summary>
    /// The user's encrypted password hash.
    /// </summary>
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// The full name of the user.
    /// </summary>
    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// The associated salt
    /// </summary>
    public Salt Salt { get; set; } = null!;

    /// <summary>
    /// A collection of roles associated with the user.
    /// </summary>
    public ICollection<Role> Roles { get; set; } = [];

    /// <summary>
    /// A collection of usages associated with the user.
    /// </summary>
    public ICollection<Usage> Usages { get; set; } = [];
}