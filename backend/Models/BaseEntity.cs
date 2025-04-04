using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

/// <summary>
/// Base class with Id, CreateAt, UpdateAt
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Record Create Time
    /// </summary>
    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Record Create Time
    /// </summary>
    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }
}