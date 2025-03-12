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
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieve all users based on query parameters
        /// </summary>
        /// <param name="query">Query parameters for filtering users</param>
        /// <returns>List of users</returns>
        /// <response code="200">Returns the list of users</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<UserDetailDTO>>> GetUsers([AsParameters] UserGetAllQueryParameters query)
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
        /// Retrieve a specific user by ID
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <returns>The user with roles</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">If the user is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithRolesDTO>> GetUser([FromRoute] Guid id)
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

        /// <summary>
        /// Update a specific user by ID
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="user">Updated user information</param>
        /// <response code="204">If the user is successfully updated</response>
        /// <response code="400">If the user data is invalid</response>
        /// <response code="404">If the user is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] Guid id, [FromBody] UserBaseDTO user)
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

        /// <summary>
        /// Update the password of a specific user by ID
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="user">User password update data</param>
        /// <response code="204">If the password is successfully updated</response>
        /// <response code="400">If the password data is invalid</response>
        /// <response code="404">If the user is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPut("{id}/pw")]
        public async Task<IActionResult> PutUserPassword([FromRoute] Guid id, [FromBody] UserUpdatPWDTO user)
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

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="body">User creation data</param>
        /// <returns>The created user with roles</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user data is invalid</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<UserWithRolesDTO>> PostUser([FromBody] UserCreateDTO body)
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

        /// <summary>
        /// Delete a specific user by ID
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <response code="204">If the user is successfully deleted</response>
        /// <response code="404">If the user is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
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
