using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

/// <summary>
/// Asset model, representing an asset in the borrowing system.
/// </summary>
public class Asset: BaseEntity
{
    /// <summary>
    /// The name of asset
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Is available to appoint
    /// </summary>
    [Column("is_available")]
    public bool isAvailable { get; set; } = true;

    /// <summary>
    /// The Other Information
    /// </summary>
    [Column("other_info")]
    public string otherInfo { get; set; } = string.Empty;

    /// <summary>
    /// A collection of usages associated with the asset.
    /// </summary>
    public ICollection<Usage> Usages { get; set; } = [];
}