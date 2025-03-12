using Microsoft.AspNetCore.Mvc;

namespace backend.Parameters;

/// <summary>
/// Query parameters for retrieving all users
/// </summary>
public class UserGetAllQueryParameters
{
    /// <summary>
    /// Filter users by role type
    /// </summary>
    [FromQuery]
    public RoleQueryEnum RoleType { get; set; } = RoleQueryEnum.All;
}

