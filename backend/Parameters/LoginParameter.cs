namespace backend.Parameters;

/// <summary>
/// Parameters for user login.
/// </summary>
public class LoginParameter
{
    /// <summary>
    /// The username of the user.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// The password of the user.
    /// </summary>
    public required string Password { get; set; }
}