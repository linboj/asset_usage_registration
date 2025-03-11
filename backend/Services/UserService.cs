using AutoMapper;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Helpers;
using Backend.Models;
using Backend.Parameters;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class UserService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserDetailDTO>> GetAll(UserGetAllQueryParameters query)
    {
        if (query.RoleType == RoleEnum.All)
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDetailDTO>>(users);
        }
        else
        {
            var users = await _context.Roles
                                .Where(r => r.Name == query.RoleType.ToString())
                                .Include(r => r.User)
                                .Select(r => r.User)
                                .ToListAsync();
            return _mapper.Map<List<UserDetailDTO>>(users);
        }
    }

    public async Task<UserWithRolesDTO?> Get(Guid id)
    {
        var user = await _context.Users
                            .Where(u => u.Id == id)
                            .Include(u => u.Roles)
                            .FirstOrDefaultAsync();
        return user == null ? null : _mapper.Map<UserWithRolesDTO>(user);
    }

    public async Task Update(Guid id, UserBaseDTO user)
    {
        if (user.Id != id)
        {
            throw new EntityValidationException(["Ids are different."]);
        }

        var existedUser = await _context.Users.FindAsync(id);

        if (existedUser == null)
        {
            throw new EntityNotFoundException(["Not found this user."]);
        }

        _context.Users.Update(existedUser).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var existedUser = await _context.Users.FindAsync(id);

        if (existedUser == null)
        {
            throw new EntityNotFoundException(["Not found this user."]);
        }

        _context.Users.Remove(existedUser);
        await _context.SaveChangesAsync();
    }

    public async Task<UserWithRolesDTO> Create(UserCreateDTO user)
    {

        if (await _context.Users.AnyAsync(u => u.UserName == user.UserName))
        {
            throw new EntityValidationException(["This UserName already exsits, Please use another one."]);
        }
        
        var passwordHash = HashingSaltingHelper.HashPasword(user.Password, out var saltValue);
        var salt = new Salt {
            Value = Convert.ToHexString(saltValue),
        };
        var userRole = new Role {
            Name = "User",
        };

        User newUser = new() {
            UserName = user.UserName,
            FullName = user.FullName,
            Salt = salt,
        };

        newUser.Roles.Add(userRole);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserWithRolesDTO>(newUser);
    }

    public async Task UpdatePassword(Guid id, UserUpdatPWDTO user)
    {
        if (user.Id != id)
        {
            throw new EntityValidationException(["Ids are different."]);
        }

        var existedUser = await _context.Users
                            .Where(u => u.Id == id)
                            .Include(u => u.Salt)
                            .SingleOrDefaultAsync();

        if (existedUser == null)
        {
            throw new EntityNotFoundException(["Not found this user."]);
        }

        bool isValid = AuthenticateUser(existedUser, user.OldPassword);

        if (!isValid){
            throw new EntityValidationException(["Old Password is wrong."]);
        }
        var passwordHash = HashingSaltingHelper.HashPasword(user.NewPassword, out var saltValue);
        var userSalt = existedUser.Salt;
        userSalt.Value = Convert.ToHexString(saltValue);
        _context.Salts.Update(userSalt);
        existedUser.PasswordHash = passwordHash;
        _context.Users.Update(existedUser);
        await _context.SaveChangesAsync();
    }

    private bool AuthenticateUser(User? user, string inputPassword)
    {
        if (user == null) return false;
        byte[] salt = Convert.FromHexString(user.Salt.Value);
        return HashingSaltingHelper.VerifyPassword(inputPassword, user.PasswordHash, salt);
    }
}