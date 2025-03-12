namespace backend.DTO;

/// <summary>
/// Data Transfer Object for creating a new asset.
/// </summary>
public class AssetCreateDTO {
    /// <summary>
    /// The name of the asset.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the asset is available.
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Additional information about the asset.
    /// </summary>
    public string OtherInfo { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for asset information.
/// </summary>
public class AssetInfoDTO: AssetCreateDTO {
    /// <summary>
    /// The unique identifier of the asset.
    /// </summary>
    public Guid Id { get; set; }
}
