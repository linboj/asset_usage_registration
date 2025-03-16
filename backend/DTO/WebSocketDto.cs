namespace backend.DTO;

public class UsageDataChangeDTO {
    /// <summary>
    /// The action of data change
    /// </summary>
    public string Action { get; set; } = "Create";
    /// <summary>
    /// The update Info
    /// </summary>
    public UsageDetailDTO? Data { get; set; } = null;
}