using System.Security.Claims;
using Backend.Exceptions;
using Backend.Helpers;
using Backend.Models;
using Backend.Parameters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class LoginService
{
    private readonly DataContext _context;

    public LoginService(DataContext context)
    {
        _context = context;
    }

    public async Task<(ClaimsIdentity claimsIdentity, AuthenticationProperties authProperties)> Login(LoginParameter input)
    {
        var user = await _context.Users.Where(u => u.UserName == input.UserName)
                   .Include(u => u.Roles)
                   .Include(u => u.Salt)
                   .Select(u => u)
                   .SingleOrDefaultAsync();

        bool isValid = AuthenticateUser(user, input.Password);

        if (!isValid || user == null)
        {
            throw new EntityNotFoundException(["user_name or password is wrong"]);
        }

        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserId", user.Id.ToString()),
                };

        var roles = user.Roles;

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var authProperties = new AuthenticationProperties
        {
            // ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(2)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return (claimsIdentity, authProperties);
    }

    private bool AuthenticateUser(User? user, string inputPassword)
    {
        if (user == null) return false;
        byte[] salt = Convert.FromHexString(user.Salt.Value);
        return HashingSaltingHelper.VerifyPassword(inputPassword, user.PasswordHash, salt);
    }
}