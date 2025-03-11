namespace Backend.DTO;

public class UserBaseDTO {
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
}

public class UserWithRolesDTO: UserDetailDTO {
    public List<RoleInfoDTO> Roles { get; set; } = [];
}

public class UserWithUsagesDTO: UserBaseDTO {
    public List<UsageWithAssetDTO> Usages { get; set; } = [];
}

public class UserDetailDTO: UserBaseDTO {
    public required string UserName { get; set; }
}

public class UserCreateDTO {
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public string FullName { get; set; } = "";
}

public class UserUpdatPWDTO{
    public Guid Id { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
