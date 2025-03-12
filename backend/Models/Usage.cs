using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

/// <summary>
/// Usage model, representing an usage in the borrowing system.
/// </summary>
public class Usage : BaseEntity
{
    /// <summary>
    /// The start datetime
    /// </summary>
    [Column("start_time")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// The end datetime
    /// </summary>
    [Column("end_time")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// The Other Information
    /// </summary>
    [Column("other_info")]
    public string OtherInfo { get; set; } = string.Empty;


    /// <summary>
    /// The associated user.
    /// </summary>
    [Required]
    [ForeignKey("User")]
    [Column("user_id")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    /// <summary>
    /// The associated user.
    /// </summary>
    [Required]
    [ForeignKey("Asset")]
    [Column("asset_id")]
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; } = null!;
}