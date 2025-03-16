using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Parameters;
using backend.DTO;
using backend.Exceptions;

namespace backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsageController : ControllerBase
    {
        private readonly UsageService _service;

        public UsageController(UsageService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieve all usages based on query parameters
        /// </summary>
        /// <param name="query">Query parameters for filtering usages</param>
        /// <returns>List of usages</returns>
        /// <response code="200">Returns the list of usages</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsageDetailDTO>>> GetUsages([AsParameters] UsageGetAllQueryParameters query)
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
        /// Retrieve a specific usage by ID
        /// </summary>
        /// <param name="id">ID of the usage</param>
        /// <returns>The usage</returns>
        /// <response code="200">Returns the usage</response>
        /// <response code="404">If the usage is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UsageDetailDTO>> GetUsage([FromRoute] Guid id)
        {
            try
            {
                var usage = await _service.Get(id);

                if (usage == null) return NotFound();

                return usage;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Update a specific usage by ID
        /// </summary>
        /// <param name="id">ID of the usage</param>
        /// <param name="usage">Updated usage information</param>
        /// <response code="204">If the usage is successfully updated</response>
        /// <response code="400">If the usage data is invalid</response>
        /// <response code="404">If the usage is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsage([FromRoute] Guid id, [FromBody] UsageInfoDTO usage)
        {
            try
            {
                await _service.Update(id, usage);
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
        /// Create a new usage
        /// </summary>
        /// <param name="body">Usage creation data</param>
        /// <returns>The created usage</returns>
        /// <response code="201">Returns the newly created usage</response>
        /// <response code="400">If the usage data is invalid</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost]
        public async Task<ActionResult<UsageDetailDTO>> PostUsage([FromBody] UsageCreateDTO body)
        {
            try
            {
                var usage = await _service.Create(body);
                return CreatedAtAction("GetUsage", new { id = usage.Id }, usage);
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
        /// Delete a specific usage by ID
        /// </summary>
        /// <param name="id">ID of the usage</param>
        /// <response code="204">If the usage is successfully deleted</response>
        /// <response code="404">If the usage is not found</response>
        /// <response code="403">If the user does not have permission to delete the usage</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsage([FromRoute] Guid id)
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
            catch (EntityValidationException ex)
            {
                return Forbid(ex.Errors[0]);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
