using backend.Parameters;

namespace backend.DTO;

/// <summary>
/// Data Transfer Object for Role information.
/// </summary>
public class RoleInfoDTO
{
    /// <summary>
    /// The unique identifier of the role.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the role.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for creating a new role.
/// </summary>
public class RoleCreateDTO {
    /// <summary>
    /// The name of the role.
    /// </summary>
    public RoleEnum Name { get; set; } = RoleEnum.User;

    /// <summary>
    /// The unique identifier of the associated user.
    /// </summary>
    public required Guid UserId { get; set; }
}