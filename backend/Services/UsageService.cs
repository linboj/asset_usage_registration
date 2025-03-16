using System.Security.Claims;
using AutoMapper;
using backend.DTO;
using backend.Exceptions;
using backend.Models;
using backend.Parameters;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class UsageService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly InfoUpateService _infoUpate;
    private readonly Guid _userId;
    private readonly bool _isManager;

    // Constructor to initialize the UsageService
    public UsageService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, InfoUpateService infoUpate)
    {
        _context = context;
        _mapper = mapper;
        _infoUpate = infoUpate;
        var Claims = httpContextAccessor.HttpContext.User.Claims.ToList();
        _userId = new Guid(Claims.Where(a => a.Type == "UserId").First().Value);
        _isManager = Claims.Where(a => a.Type == ClaimTypes.Role).Any(r => r.Value == "Manager");
    }

    /// <summary>
    /// Retrieve all usages based on query parameters
    /// </summary>
    /// <param name="query">Query parameters for filtering usages</param>
    /// <returns>List of usages</returns>
    public async Task<List<UsageDetailDTO>> GetAll(UsageGetAllQueryParameters query)
    {
        var search = _context.Usages
                            .Where(us => us.EndTime >= query.StartDate || us.StartTime <= query.EndDate);
        if (query.UserId != null)
        {
            search = search.Where(us => us.UserId == query.UserId).Include(us => us.Asset);
        }
        if (query.AssetId != null)
        {
            search = search.Where(us => us.AssetId == query.AssetId).Include(us => us.User);
        }
        var usages = await search.ToListAsync();
        return _mapper.Map<List<UsageDetailDTO>>(usages);
    }

    /// <summary>
    /// Retrieve a specific usage by ID
    /// </summary>
    /// <param name="id">ID of the usage</param>
    /// <returns>The usage</returns>
    public async Task<UsageDetailDTO?> Get(Guid id)
    {
        var usage = await _context.Usages
                            .Where(us => us.Id == id)
                            .Include(us => us.User)
                            .Include(us => us.Asset)
                            .FirstOrDefaultAsync();
        return usage == null ? null : _mapper.Map<UsageDetailDTO>(usage);
    }

    /// <summary>
    /// Update a specific usage by ID
    /// </summary>
    /// <param name="id">ID of the usage</param>
    /// <param name="usage">Updated usage information</param>
    public async Task Update(Guid id, UsageInfoDTO usage)
    {
        if (usage.Id != id)
        {
            throw new EntityValidationException(["Ids are different."]);
        }

        var existedUsage = await _context.Usages.FindAsync(id);

        if (existedUsage == null)
        {
            throw new EntityNotFoundException(["Not found this usage."]);
        }

        if (existedUsage.UserId != _userId && !_isManager)
        {
            throw new EntityValidationException(["No delete permission"]);
        }

        if (await IsAssetUsedInDateTimeRange(usage.StartTime, usage.EndTime, usage.AssetId, id))
        {
            throw new EntityValidationException(["During this period, the asset is used by others"]);
        }

        _context.Usages.Update(existedUsage).CurrentValues.SetValues(usage);
        await _context.SaveChangesAsync();
        var updatedInfo = _mapper.Map<UsageDetailDTO>(existedUsage);
        var dataChange = new UsageDataChangeDTO { Data = updatedInfo, Action = "Update" };
        _ = _infoUpate.SendUsageUpdateInfo(updatedInfo.AssetId.ToString(), dataChange);
    }

    /// <summary>
    /// Delete a specific usage by ID
    /// </summary>
    /// <param name="id">ID of the usage</param>
    public async Task Delete(Guid id)
    {
        var existedUsage = await _context.Usages.FindAsync(id);

        if (existedUsage == null)
        {
            throw new EntityNotFoundException(["Not found this usage."]);
        }

        if (existedUsage.UserId != _userId && !_isManager)
        {
            throw new EntityValidationException(["No delete permission"]);
        }
        var assetId = existedUsage.AssetId;
        _context.Usages.Remove(existedUsage);
        await _context.SaveChangesAsync();
        var dataChange = new UsageDataChangeDTO { Data = new UsageDetailDTO { Id = id }, Action = "Delete" };
        _ = _infoUpate.SendUsageUpdateInfo(assetId.ToString(), dataChange);
    }

    /// <summary>
    /// Create a new usage
    /// </summary>
    /// <param name="usage">Usage creation data</param>
    /// <returns>The created usage</returns>
    public async Task<UsageDetailDTO> Create(UsageCreateDTO usage)
    {
        if (await IsAssetUsedInDateTimeRange(usage.StartTime, usage.EndTime, usage.AssetId))
        {
            throw new EntityValidationException(["During this period, the asset is used by others"]);
        }

        usage.UserId = _userId;

        var newUsage = _mapper.Map<Usage>(usage);

        _context.Usages.Add(newUsage);
        await _context.SaveChangesAsync();
        var updatedInfo = _mapper.Map<UsageDetailDTO>(newUsage);
        var dataChange = new UsageDataChangeDTO { Data = updatedInfo, Action = "Create" };
        _ = _infoUpate.SendUsageUpdateInfo(updatedInfo.AssetId.ToString(), dataChange);

        return updatedInfo;
    }

    // Method to check if an asset is used in a specific date-time range
    private async Task<bool> IsAssetUsedInDateTimeRange(DateTime startTime, DateTime endTime, Guid assetId, Guid? usageId = null)
    {
        var search = _context.Usages
                        .Where(us => us.StartTime <= startTime && us.EndTime >= startTime)
                        .Where(us => us.StartTime <= endTime && us.EndTime >= endTime)
                        .Where(us => us.AssetId == assetId);
        if (usageId != null)
        {
            search = search.Where(us => us.Id != usageId);
        }
        return await search.AnyAsync();
    }
}