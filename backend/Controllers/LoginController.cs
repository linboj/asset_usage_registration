using Microsoft.AspNetCore.Mvc;
using backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using backend.Parameters;
using backend.Exceptions;

namespace backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _service;

        public LoginController(LoginService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns 401 Unauthorized status.
        /// </summary>
        /// <returns>401 Unauthorized status.</returns>
        [HttpGet("401")]
        public ActionResult Unauthorized401()
        {
            return Unauthorized();
        }

        /// <summary>
        /// Returns 403 Forbidden status.
        /// </summary>
        /// <returns>403 Forbidden status.</returns>
        [HttpGet("403")]
        public ActionResult Forbidden403()
        {
            return Forbid();
        }

        /// <summary>
        /// Handles user login.
        /// </summary>
        /// <param name="input">The login parameters.</param>
        /// <returns>Ok status if login is successful, otherwise appropriate error status.</returns>
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginParameter input)
        {
            try
            {
                var (claimsIdentity, authProperties) = await _service.Login(input);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Handles user logout.
        /// </summary>
        [HttpDelete]
        public void logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
