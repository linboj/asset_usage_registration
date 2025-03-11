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
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<UserDetailDTO>>> GetUsers(UserGetAllQueryParameters query)
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

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithRolesDTO>> GetUser(Guid id)
        {
            try
            {
                var user = await _service.Get(id);

                if (user == null) return NotFound();

                return user;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserBaseDTO user)
        {
            try
            {
                await _service.Update(id, user);
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

        [HttpPut("{id}/pw")]
        public async Task<IActionResult> PutUser(Guid id, UserUpdatPWDTO user)
        {
            try
            {
                await _service.UpdatePassword(id, user);
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

        // POST: api/User
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<User>> PostUser(UserCreateDTO body)
        {
            try
            {
                var user = await _service.Create(body);
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
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

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUser(Guid id)
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
