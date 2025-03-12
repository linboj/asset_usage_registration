using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Parameters;
using backend.DTO;
using backend.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class AssetController : ControllerBase
    {
        private readonly AssetService _service;

        // Constructor to initialize the AssetService
        public AssetController(AssetService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieve all assets based on query parameters
        /// </summary>
        /// <param name="query">Query parameters for filtering assets</param>
        /// <returns>List of assets</returns>
        /// <response code="200">Returns the list of assets</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetInfoDTO>>> GetAssets([AsParameters] AssetGetAllQueryParameters query)
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

        /// <summary>
        /// Retrieve a specific asset by ID
        /// </summary>
        /// <param name="id">ID of the asset</param>
        /// <returns>The asset</returns>
        /// <response code="200">Returns the asset</response>
        /// <response code="404">If the asset is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetInfoDTO>> GetAsset([FromRoute] Guid id)
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

        /// <summary>
        /// Update a specific asset by ID
        /// </summary>
        /// <param name="id">ID of the asset</param>
        /// <param name="asset">Updated asset information</param>
        /// <response code="204">If the asset is successfully updated</response>
        /// <response code="400">If the asset data is invalid</response>
        /// <response code="404">If the asset is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutAsset([FromRoute] Guid id, [FromBody] AssetInfoDTO asset)
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

        /// <summary>
        /// Create a new asset
        /// </summary>
        /// <param name="body">Asset creation data</param>
        /// <returns>The created asset</returns>
        /// <response code="201">Returns the newly created asset</response>
        /// <response code="400">If the asset data is invalid</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<AssetInfoDTO>> PostAsset([FromBody] AssetCreateDTO body)
        {
            try
            {
                var asset = await _service.Create(body);
                return CreatedAtAction("GetAsset", new { id = asset.Id }, asset);
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

        /// <summary>
        /// Delete a specific asset by ID
        /// </summary>
        /// <param name="id">ID of the asset</param>
        /// <response code="204">If the asset is successfully deleted</response>
        /// <response code="404">If the asset is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteAsset([FromRoute] Guid id)
        {
            try
            {
                await _service.Delete(id);
                return NoContent();
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
    }
}
