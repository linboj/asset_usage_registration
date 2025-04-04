using AutoMapper;
using backend.DTO;
using backend.Exceptions;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class RoleService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public RoleService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all roles by User ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The role information.</returns>
    public async Task<List<string>> GetAllOfUser(Guid userId)
    {
        var roles = await _context.Roles
                            .Where(r => r.UserId == userId)
                            .Select(r => r.Name)
                            .ToListAsync();
        return roles;
    }

    /// <summary>
    /// Get a role by ID.
    /// </summary>
    /// <param name="id">The ID of the role.</param>
    /// <returns>The role information.</returns>
    public async Task<RoleInfoDTO?> Get(Guid id)
    {
        var role = await _context.Roles
                            .Where(r => r.Id == id)
                            .FirstOrDefaultAsync();
        return role == null ? null : _mapper.Map<RoleInfoDTO>(role);
    }

    /// <summary>
    /// Delete a role by ID.
    /// </summary>
    /// <param name="id">The ID of the role.</param>
    public async Task Delete(Guid id)
    {
        var existedRole = await _context.Roles.FindAsync(id);

        if (existedRole == null)
        {
            throw new EntityNotFoundException(["Not found this role."]);
        }

        _context.Roles.Remove(existedRole);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Create a new role.
    /// </summary>
    /// <param name="input">The role creation data.</param>
    /// <returns>The created role information.</returns>
    public async Task<RoleInfoDTO> Create(RoleCreateDTO input)
    {
        var isWithSameRole = await _context.Roles.Where(r => r.Name == input.Name.ToString() && r.UserId == input.UserId).AnyAsync();

        if (isWithSameRole) {
            throw new EntityValidationException(["User already had this role"]);
        }

        Role Role = _mapper.Map<Role>(input);
        _context.Roles.Add(Role);
        await _context.SaveChangesAsync();

        return _mapper.Map<RoleInfoDTO>(Role);
    }
}