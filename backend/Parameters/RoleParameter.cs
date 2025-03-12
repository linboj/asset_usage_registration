namespace backend.Parameters;

/// <summary>
/// Enumeration for different roles.
/// </summary>
public enum RoleEnum {
    /// <summary>
    /// Regular user role.
    /// </summary>
    User = 1,
    /// <summary>
    /// Manager role.
    /// </summary>
    Manager = 2,
}

/// <summary>
/// Enumeration for role queries.
/// </summary>
public enum RoleQueryEnum
{
    /// <summary>
    /// Query for all roles.
    /// </summary>
    All = 0,
    /// <summary>
    /// Query for user roles.
    /// </summary>
    User = 1,
    /// <summary>
    /// Query for manager roles.
    /// </summary>
    Manager = 2,
}