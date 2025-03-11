namespace Backend.DTO;

public class AssetCreateDTO {
    public string Name { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public string OtherInfo { get; set; } = string.Empty;
}
public class AssetInfoDTO: AssetCreateDTO {
    public Guid Id { get; set; }
}


public class AssetInfoWithUsage: AssetInfoDTO {
    public List<UsageWithUserDTO> Usages { get; set; } = [];
}
