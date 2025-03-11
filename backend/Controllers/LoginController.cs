using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Backend.Parameters;
using Backend.Exceptions;

namespace Backend.Controllers
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

        [Route("401")]
        public ActionResult Unauthorized401()
        {
            return Unauthorized();
        }

        [Route("403")]
        public ActionResult Forbidden403()
        {
            return Forbid();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginParameter input)
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

        [HttpDelete]
        public void logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
