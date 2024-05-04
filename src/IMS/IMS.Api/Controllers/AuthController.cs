using IMS.Api.RequestHandlers;
using IMS.Infrastructure.Membership;
using IMS.Infrastructure.Membership.Enums;
using IMS.Infrastructure.Membership.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager, ITokenService tokenService) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequestHandler request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            var result = await userManager.CreateAsync(
                new AppUser { UserName = request.Username, Email = request.Email, Role = Role.User },
                request.Password!
            );

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(Register), new { email = request.Email, role = request.Role }, request.Email);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequestHandler request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await userManager.FindByEmailAsync(request.Email!);
            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(managedUser, request.Password!);
            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var accessToken = await tokenService.CreateTokenAsync(managedUser);
        
            return Ok(new AuthResponse
            {
                Email = managedUser.Email,
                Token = accessToken.Token,
            });
        }
    }
}
