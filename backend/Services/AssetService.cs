using AutoMapper;
using Backend.DTO;
using Backend.Exceptions;
using Backend.Models;
using Backend.Parameters;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AssetService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AssetService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

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

    public async Task<AssetInfoDTO?> Get(Guid id)
    {
        var asset = await _context.Assets
                            .Where(a => a.Id == id)
                            .FirstOrDefaultAsync();
        return asset == null ? null : _mapper.Map<AssetInfoDTO>(asset);
    }

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

    public async Task<AssetInfoDTO> Create(AssetCreateDTO input)
    {
        Asset asset = _mapper.Map<Asset>(input);
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        return _mapper.Map<AssetInfoDTO>(asset);
    }
}