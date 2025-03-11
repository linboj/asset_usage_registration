namespace Backend.Parameters;

public class AssetGetAllQueryParameters
{
    public bool? IsAvailable { get; set; } = null;
}

public class AssetGetQueryParameters
{
    private static DateTime Today = DateTime.Today;
    public DateTime StartDate { get; set; } = new DateTime(Today.Year, Today.Month, 1);
    public DateTime EndDate { get; set; } = new DateTime(Today.Year, Today.Month, 1).AddMonths(1);
}

