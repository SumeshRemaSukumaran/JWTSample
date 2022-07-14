using JWTModels.Dto;
using JWTSample.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.Threading.Tasks;

namespace JWTSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthentication _authentication;

        public AuthenticationController(IConfiguration configuration, IAuthentication authentication)
        {
            _config = configuration;
            _authentication = authentication;
        }

        [HttpPost]
        [Route("login")]
        public async Task< IActionResult> UserLogin([FromBody] UserLoginDto userLogin)
        {         
            (bool isValiduser, string token) = await _authentication.AuthenticateUser(userLogin);
            if (isValiduser)
            {
                return Ok(token);
            }
            return NotFound(token);
        }
    }
}
