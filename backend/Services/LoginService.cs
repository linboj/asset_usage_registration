using System.Security.Claims;
using backend.Exceptions;
using backend.Helpers;
using backend.Models;
using backend.Parameters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

/// <summary>
/// Service to handle user login operations.
/// </summary>
public class LoginService
{
    private readonly DataContext _context;

    /// <summary>
    /// Constructor to initialize the DataContext.
    /// </summary>
    /// <param name="context">The data context.</param>
    public LoginService(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Method to handle user login.
    /// </summary>
    /// <param name="input">The login parameters.</param>
    /// <returns>A tuple containing ClaimsIdentity and AuthenticationProperties.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the user is not found or the password is incorrect.</exception>
    public async Task<(ClaimsIdentity claimsIdentity, AuthenticationProperties authProperties)> Login(LoginParameter input)
    {
        // Retrieve the user from the database based on the provided username
        var user = await _context.Users.Where(u => u.UserName == input.UserName)
                   .Include(u => u.Roles)
                   .Include(u => u.Salt)
                   .Select(u => u)
                   .SingleOrDefaultAsync();

        // Validate the user's password
        bool isValid = AuthenticateUser(user, input.Password);

        // If the user is not found or the password is invalid, throw an exception
        if (!isValid || user == null)
        {
            throw new EntityNotFoundException(["user_name or password is wrong"]);
        }

        // Create claims for the authenticated user
        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserId", user.Id.ToString()),
                };

        // Add roles to the claims
        var roles = user.Roles;
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        // Set authentication properties
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
            IsPersistent = true,
        };

        // Create a ClaimsIdentity with the claims and authentication scheme
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Return the ClaimsIdentity and AuthenticationProperties
        return (claimsIdentity, authProperties);
    }

    /// <summary>
    /// Method to authenticate the user by verifying the password.
    /// </summary>
    /// <param name="user">The user entity.</param>
    /// <param name="inputPassword">The input password.</param>
    /// <returns>True if the password is valid, otherwise false.</returns>
    private bool AuthenticateUser(User? user, string inputPassword)
    {
        if (user == null) return false;
        byte[] salt = Convert.FromHexString(user.Salt.Value);
        return HashingSaltingHelper.VerifyPassword(inputPassword, user.PasswordHash, salt);
    }
}