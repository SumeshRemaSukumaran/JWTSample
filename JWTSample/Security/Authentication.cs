using JWTModels.Dto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTSample.Security
{
    public class Authentication : IAuthentication
    {
        private readonly TokenManagementDto _tokenManagement;

        public Authentication(IOptions<TokenManagementDto> tokenManagementDto)
        {
            _tokenManagement = tokenManagementDto.Value;
        }

        public async Task<(bool, string)> AuthenticateUser(UserLoginDto userLogin)
        {
            string token = string.Empty;
            byte[] data = Convert.FromBase64String(userLogin.Password);
            userLogin.Password = Encoding.UTF8.GetString(data);

            var user = Users.RegistredUsers.FirstOrDefault(s => s.UserName == userLogin.UserName && s.Password == userLogin.Password);
            if (user == null)
            {
                return (false, "AuthenticationUserNotFound");
            }
            else
            {
                var sessionId = Guid.NewGuid().ToString();
                var claim = new[]
                {
                    new Claim("name", user.GiveName),
                    new Claim("sessionId", sessionId),
                };
                token = GenerateToken(claim);
                return (true, token);
            }           
        }

        public Task<(bool, string)> GenerateAuthToken()
        {
            throw new System.NotImplementedException();
        }

        public Task<(bool, string)> GenerateRefreshToken(string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> LogoutUser(string sessionId)
        {
            throw new System.NotImplementedException();
        }

        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var expiryMinutes = Convert.ToInt16(_tokenManagement.AccessExpiration);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claims,
                 expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                 signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
