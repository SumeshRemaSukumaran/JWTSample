using JWTSample.API.Entities;
using JWTSample.Contract.Dto;
using JWTSample.Contract.ViewModel;
using JWTSample.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWTSample.Service.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly TokenManagementDto _tokenManagement;

        public AuthenticationService(IOptions<TokenManagementDto> tokenManagementDto)
        {
            _tokenManagement = tokenManagementDto.Value;
        }

        public async Task<AuthenticationModel> AuthenticateUser(UserLoginDto userLogin)
        {
            string token = string.Empty;
            byte[] data = Convert.FromBase64String(userLogin.Password);
            userLogin.Password = Encoding.UTF8.GetString(data);
            AuthenticationModel authenticationModel = new AuthenticationModel();

            var user = Users.RegistredUsers.FirstOrDefault(s => s.UserName == userLogin.UserName && s.Password == userLogin.Password);
            if (user == null)
            {
                authenticationModel.Message = "User Not Found";
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Token = string.Empty;
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

                authenticationModel.SessionId= sessionId;
                authenticationModel.Message = "Authenticated";
                authenticationModel.IsAuthenticated = true;
                authenticationModel.Token = token;
                authenticationModel.UserName = user.GiveName;
                if (string.IsNullOrEmpty(authenticationModel.RefreshToken))
                {
                    var refreshToken = await GenerateRefreshToken(sessionId);
                    Users.RefreshTokens.Add(refreshToken);
                    authenticationModel.RefreshToken = refreshToken.Token;
                    authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                }
                else
                {
                    var refreshToken = Users.RefreshTokens.FirstOrDefault(s=> s.SessionId== authenticationModel.SessionId);
                    authenticationModel.RefreshToken = refreshToken.Token;
                    authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                }
                SaveUserSessions(authenticationModel);
            }
            return authenticationModel;
        }

        public async Task<AuthenticationModel> GenerateAuthToken(string refreshToken)
        {
            var rToken = Users.RefreshTokens.FirstOrDefault(s => s.Token == refreshToken);
            AuthenticationModel authenticationModel = new AuthenticationModel();
            if (rToken?.IsExpired == false)
            {
                var claim = new[]
               {
                    new Claim("sessionId", rToken.SessionId),
                };
               var  token = GenerateToken(claim);
               
                authenticationModel.SessionId = rToken.SessionId;
                authenticationModel.Message = "Authenticated";
                authenticationModel.IsAuthenticated = true;
                authenticationModel.Token = token;
                ClearUserSessions(rToken.SessionId);
                SaveUserSessions(authenticationModel);
            }
            else
            {
                authenticationModel.Message = "Refresh Token Expired";
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Token = string.Empty;
            }

            return authenticationModel;
        }

        public async Task<RefreshToken> GenerateRefreshToken(string sessionId)
        {
            var randomNumber = new byte[32];
            string refreshToken;
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
                var expiryMinutes = Convert.ToInt16(_tokenManagement.RefreshExpiratonMinutes);
                return new RefreshToken
                {
                    Token = refreshToken,
                    Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                    SessionId = sessionId,
                };
            }
        }

        public async Task<bool> LogoutUser(string sessionId)
        {
            ClearUserSessions(sessionId);
            return true;
        }

        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var expiryMinutes = Convert.ToInt16(_tokenManagement.AccessExpirationMinutes);
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
        private void SaveUserSessions(AuthenticationModel authenticationModel )
        {
            Users.UserSessions.Add(authenticationModel);
        }
        private void ClearUserSessions(string sessionId)
        {
            var item = Users.UserSessions.FirstOrDefault(s => s.SessionId == sessionId);
            Users.UserSessions.Remove(item);
        }
    }
}
