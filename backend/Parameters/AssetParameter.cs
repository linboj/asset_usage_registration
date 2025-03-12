using Microsoft.AspNetCore.Mvc;

namespace backend.Parameters;

/// <summary>
/// Query parameters for retrieving all assets
/// </summary>
public class AssetGetAllQueryParameters
{
    /// <summary>
    /// Filter assets by availability
    /// </summary>
    [FromQuery]
    public bool? IsAvailable { get; set; } = null;
}

/// <summary>
/// Query parameters for retrieving a specific asset
/// </summary>
public class AssetGetQueryParameters
{
    private static DateTime Today = DateTime.Today;

    /// <summary>
    /// Start date for the asset query
    /// </summary>
    [FromQuery]
    public DateTime StartDate { get; set; } = new DateTime(Today.Year, Today.Month, 1);

    /// <summary>
    /// End date for the asset query
    /// </summary>
    [FromQuery]
    public DateTime EndDate { get; set; } = new DateTime(Today.Year, Today.Month, 1).AddMonths(1);
}

