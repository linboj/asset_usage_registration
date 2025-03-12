using AutoMapper;
using backend.DTO;
using backend.Exceptions;
using backend.Models;
using backend.Parameters;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

/// <summary>
/// Service to handle asset operations.
/// </summary>
public class AssetService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AssetService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all assets based on query parameters.
    /// </summary>
    /// <param name="query">The query parameters for filtering assets.</param>
    /// <returns>A list of asset information.</returns>
    public async Task<List<AssetInfoDTO>> GetAll(AssetGetAllQueryParameters query)
    {
        var search = _context.Assets.Select(a => a);

        if (query.IsAvailable == true)
            search = search.Where(a => a.IsAvailable);
        else if (query.IsAvailable == false)
            search = search.Where(a => !a.IsAvailable);
        var assets = await search.ToListAsync();
        return _mapper.Map<List<AssetInfoDTO>>(assets);
    }

    /// <summary>
    /// Get an asset by ID.
    /// </summary>
    /// <param name="id">The ID of the asset.</param>
    /// <returns>The asset information.</returns>
    public async Task<AssetInfoDTO?> Get(Guid id)
    {
        var asset = await _context.Assets
                            .Where(a => a.Id == id)
                            .FirstOrDefaultAsync();
        return asset == null ? null : _mapper.Map<AssetInfoDTO>(asset);
    }

    /// <summary>
    /// Update an asset by ID.
    /// </summary>
    /// <param name="id">The ID of the asset.</param>
    /// <param name="asset">The asset information to update.</param>
    /// <exception cref="EntityValidationException">Thrown when the IDs do not match.</exception>
    /// <exception cref="EntityNotFoundException">Thrown when the asset is not found.</exception>
    public async Task Update(Guid id, AssetInfoDTO asset)
    {
        if (asset.Id != id)
        {
            throw new EntityValidationException(["Ids are different."]);
        }

        var existedAsset = await _context.Assets.FindAsync(id);

        if (existedAsset == null)
        {
            throw new EntityNotFoundException(["Not found this asset."]);
        }

        _context.Assets.Update(existedAsset).CurrentValues.SetValues(asset);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete an asset by ID.
    /// </summary>
    /// <param name="id">The ID of the asset.</param>
    /// <exception cref="EntityNotFoundException">Thrown when the asset is not found.</exception>
    public async Task Delete(Guid id)
    {
        var existedAsset = await _context.Assets.FindAsync(id);

        if (existedAsset == null)
        {
            throw new EntityNotFoundException(["Not found this asset."]);
        }

        _context.Assets.Remove(existedAsset);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Create a new asset.
    /// </summary>
    /// <param name="input">The asset creation data.</param>
    /// <returns>The created asset information.</returns>
    public async Task<AssetInfoDTO> Create(AssetCreateDTO input)
    {
        Asset asset = _mapper.Map<Asset>(input);
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        return _mapper.Map<AssetInfoDTO>(asset);
    }
}