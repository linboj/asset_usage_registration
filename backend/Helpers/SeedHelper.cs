using backend.DTO;
using backend.Models;
using backend.Parameters;
using backend.Services;

namespace backend.Helpers;

/// <summary>
/// Helper class to seed initial data into the database.
/// </summary>
public class SeedHelper
{
    private readonly DataContext _context;
    private readonly UserService _service;

    /// <summary>
    /// Constructor to initialize the DataContext and UserService.
    /// </summary>
    /// <param name="context">The data context.</param>
    /// <param name="service">The user service.</param>
    public SeedHelper(DataContext context, UserService service)
    {
        _context = context;
        _service = service;
    }

    /// <summary>
    /// Method to seed initial data into the database.
    /// </summary>
    public async Task SeedAsync()
    {
        var Env = Environment.GetEnvironmentVariables();
        if (!_context.Users.Any())
        {
            var input = new UserCreateDTO
            {
                UserName = Env["INIT_USER_NAME"] == null ? "1stManager" : $"{Env["INIT_USER_NAME"]}",
                Password = Env["INIT_PASSWORD"] == null ? "ThisI$APA$$W0rd" : $"{Env["INIT_PASSWORD"]}",
                FullName = Env["INIT_FULL_NAME"] == null ? "1stManager" : $"{Env["INIT_FULL_NAME"]}",
            };

            // Create the initial user
            var user = await _service.Create(input);

            // Assign the Manager role to the created user
            var managerRole = new Role
            {
                Name = RoleEnum.Manager.ToString(),
                UserId = user.Id,
            };

            // Add the role to the context and save changes
            _context.Roles.Add(managerRole);
            await _context.SaveChangesAsync();
        }
    }
}