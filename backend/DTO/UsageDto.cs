namespace backend.DTO;

/// <summary>
/// Base DTO for usage information
/// </summary>
public class UsageBaseDTO
{
    /// <summary>
    /// Start time of the usage
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// End time of the usage
    /// </summary>
    public DateTime EndTime { get; set; }
    /// <summary>
    /// Additional information about the usage
    /// </summary>
    public string OtherInfo { get; set; } = string.Empty;
}

/// <summary>
/// DTO for creating a new usage
/// </summary>
public class UsageCreateDTO: UsageBaseDTO
{
    /// <summary>
    /// User ID associated with the usage
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Asset ID associated with the usage
    /// </summary>
    public Guid AssetId { get; set; }
}

/// <summary>
/// DTO for usage information with ID
/// </summary>
public class UsageInfoDTO : UsageCreateDTO
{
    /// <summary>
    /// Usage ID
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// DTO for detailed usage information
/// </summary>
public class UsageDetailDTO : UsageInfoDTO
{
    /// <summary>
    /// Asset information associated with the usage
    /// </summary>
    public AssetInfoDTO? Asset { get; set; }
    /// <summary>
    /// User information associated with the usage
    /// </summary>
    public UserBaseDTO? User { get; set; }
}