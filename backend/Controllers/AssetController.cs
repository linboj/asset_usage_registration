using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using Backend.Parameters;
using Backend.DTO;
using Backend.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class AssetController : ControllerBase
    {
        private readonly AssetService _service;

        public AssetController(AssetService service)
        {
            _service = service;
        }

        // GET: api/Asset
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetInfoDTO>>> GetAssets(AssetGetAllQueryParameters query)
        {
            try
            {
                return await _service.GetAll(query);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: api/Asset/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetInfoDTO>> GetAsset(Guid id)
        {
            try
            {
                var asset = await _service.Get(id);

                if (asset == null) return NotFound();

                return asset;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/Asset/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutAsset(Guid id, AssetInfoDTO asset)
        {
            try
            {
                await _service.Update(id, asset);
                return NoContent();
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST: api/Asset
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<AssetInfoDTO>> PostAsset(AssetCreateDTO body)
        {
            try
            {
                var Asset = await _service.Create(body);
                return CreatedAtAction("GetAsset", new { id = Asset.Id }, Asset);
            }
            catch (EntityValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
