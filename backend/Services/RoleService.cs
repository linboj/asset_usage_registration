using AutoMapper;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class RoleService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public RoleService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleInfoDTO?> Get(Guid id)
    {
        var role = await _context.Roles
                            .Where(a => a.Id == id)
                            .FirstOrDefaultAsync();
        return role == null ? null : _mapper.Map<RoleInfoDTO>(role);
    }

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