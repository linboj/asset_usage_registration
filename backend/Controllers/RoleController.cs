using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTO;
using backend.Services;
using backend.Exceptions;
using backend.Parameters;

namespace backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _service;

        public RoleController(RoleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get a role by ID.
        /// </summary>
        /// <param name="id">The ID of the role.</param>
        /// <returns>The role information.</returns>
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetRolesOfUser([FromRoute] Guid id)
        {
            try
            {
                var claims = HttpContext.User.Claims.ToList();
                var userId = claims.Where(a => a.Type == "UserId").First().Value;
                if (userId == null) return Unauthorized();
                var roles = await _service.GetAllOfUser(Guid.Parse(userId));

                return roles;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Get a role by ID.
        /// </summary>
        /// <param name="id">The ID of the role.</param>
        /// <returns>The role information.</returns>
        [Authorize(Roles = "Manager")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleInfoDTO>> GetRole([FromRoute] Guid id)
        {
            try
            {
                var role = await _service.Get(id);

                if (role == null) return NotFound();

                return role;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="body">The role creation data.</param>
        /// <returns>The created role information.</returns>
        [Authorize(Roles = "Manager")]
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

        /// <summary>
        /// Delete a role by ID.
        /// </summary>
        /// <param name="id">The ID of the role.</param>
        /// <returns>No content if the deletion is successful.</returns>
        [Authorize(Roles = "Manager")]
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

        /// <summary>
        /// Get all role type.
        /// </summary>
        /// <returns>An array with all role types.</returns>
        [HttpGet("role_type")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllRoleType()
        {
            try
            {
                var roleTypes = Enum.GetNames(typeof(RoleEnum));
                return roleTypes;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
