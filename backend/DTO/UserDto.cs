namespace backend.DTO;

/// <summary>
/// Base DTO for user information
/// </summary>
public class UserBaseDTO {
    /// <summary>
    /// User ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Full name of the user
    /// </summary>
    public string FullName { get; set; } = "";
}

/// <summary>
/// DTO for user information with roles
/// </summary>
public class UserWithRolesDTO: UserDetailDTO {
    /// <summary>
    /// List of roles assigned to the user
    /// </summary>
    public List<RoleInfoDTO> Roles { get; set; } = [];
}

public class UserDetailDTO: UserBaseDTO {
    /// <summary>
    /// Username of the user
    /// </summary>
    public required string UserName { get; set; }
}

/// <summary>
/// DTO for creating a new user
/// </summary>
public class UserCreateDTO {
    /// <summary>
    /// Username of the new user
    /// </summary>
    public required string UserName { get; set; }
    /// <summary>
    /// Password of the new user
    /// </summary>
    public required string Password { get; set; }
    /// <summary>
    /// Full name of the new user
    /// </summary>
    public string FullName { get; set; } = "";
}

/// <summary>
/// DTO for updating user password
/// </summary>
public class UserUpdatPWDTO{
    /// <summary>
    /// User ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Old password of the user
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;
    /// <summary>
    /// New password of the user
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}
