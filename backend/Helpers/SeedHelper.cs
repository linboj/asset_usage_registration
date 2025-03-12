using backend.DTO;
using backend.Models;
using backend.Parameters;
using backend.Services;

namespace backend.Helpers;

public class SeedHelper
{
    private readonly DataContext _context;
    private readonly UserService _service;

    public SeedHelper(DataContext context, UserService service)
    {
        _context = context;
        _service = service;
    }

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

            var user = await _service.Create(input);

            var managerRole = new Role
            {
                Name = RoleEnum.Manager.ToString(),
                UserId = user.Id,
            };

            _context.Roles.Add(managerRole);
            await _context.SaveChangesAsync();
        }
    }
}