using Backend.Parameters;

namespace Backend.DTO;

public class RoleInfoDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class RoleCreateDTO {
    public RoleEnum Name { get; set; } = RoleEnum.User;
    public required Guid UserId { get; set; }
}