using JWTSample.Contract.Dto;
using JWTSample.Service.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;

namespace JWTSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthenticationService _authentication;

        public AuthenticationController(IConfiguration configuration, IAuthenticationService authentication)
        {
            _config = configuration;
            _authentication = authentication;
        }

        [HttpPost]
        [Route("login")]
        public async Task< IActionResult> UserLogin([FromBody] UserLoginDto userLogin)
        {         
            var authModel = await _authentication.AuthenticateUser(userLogin);
            if (authModel.IsAuthenticated)
            {
                SetRefreshTokenInCookies(authModel.RefreshToken);
                return Ok(authModel);
            }
            return NotFound(authModel.Token);
        }

        private void SetRefreshTokenInCookies(string refreshToken)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(2),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOption);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var token = await _authentication.GenerateAuthToken(refreshToken);
            return Ok(token);
        }

        [HttpDelete]
        [Route("logout")]
        public async Task<IActionResult> Logout(string sessionId)
        {
            var isLogout = _authentication.LogoutUser(sessionId);
            return Ok(isLogout);
        }
    }
}
