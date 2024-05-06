using System.Net;
using IMS.Infrastructure.Membership;
using IMS.Infrastructure.Membership.Tokens;
using IMS.Infrastructure.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(ITokenService tokenService) : ApiControllerBase
    {
        [HttpPost("get-token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] AppUser user)
        {
            var token = await tokenService.CreateTokenAsync(user);
            if (token is null)
            {
                return BadRequest(new { message = "Invalid username or password" });
            }

            return await SendResponseAsync<TokenResponse, TokenResponse>(HttpStatusCode.OK, token);
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var response = await tokenService.RefreshTokenAsync(request);
            if (response == null)
            {
                return BadRequest(new { message = "Invalid token" });
            }

            return Ok(response);
        }
    }
}
