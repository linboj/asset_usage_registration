using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.DTO;
using Backend.Services;
using Backend.Exceptions;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _service;

        public RoleController(RoleService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleInfoDTO>> GetRole([FromRoute] Guid id)
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

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<RoleInfoDTO>> PostRole([FromBody] RoleCreateDTO body)
        {
            try
            {
                var role = await _service.Create(body);
                return CreatedAtAction("GetAsset", new { id = role.Id }, role);
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

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
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
