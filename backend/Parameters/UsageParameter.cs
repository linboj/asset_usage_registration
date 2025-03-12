using Microsoft.AspNetCore.Mvc;

namespace backend.Parameters;

/// <summary>
/// Query parameters for retrieving all usages
/// </summary>
public class UsageGetAllQueryParameters
{
    private static DateTime Today = DateTime.Today;

    /// <summary>
    /// Filter usages by user ID
    /// </summary>
    [FromQuery]
    public Guid? UserId { get; set; } = null;

    /// <summary>
    /// Filter usages by asset ID
    /// </summary>
    [FromQuery]
    public Guid? AssetId { get; set; } = null;

    /// <summary>
    /// Start date for the usage query
    /// </summary>
    [FromQuery]
    public DateTime StartDate { get; set; } = new DateTime(Today.Year, Today.Month, 1);

    /// <summary>
    /// End date for the usage query
    /// </summary>
    [FromQuery]
    public DateTime EndDate { get; set; } = new DateTime(Today.Year, Today.Month, 1).AddMonths(1);
}

