using JWTSample.Contract.Dto;
using JWTSample.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text;

namespace JWTSample.Service.Security
{
    public class ResourceAccessProvider : IResourceAccessProvider
    {
        public bool ValidateToken(string authToken, TokenManagementDto tokenManagementDto, bool checkExpiry)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            //var decodedToken = tokenHandler.ReadJwtToken(authToken);
            //var sessionId = decodedToken.Payload.FirstOrDefault(s => s.Key == "sessionId").Value.ToString();
            SecurityToken validatedToken = null;
            if (Users.UserSessions.FirstOrDefault(s => s.Token == authToken) != null)
            {
                var validationParameter = GetTokenValidationParameters(tokenManagementDto, checkExpiry);
                try
                {
                    IPrincipal userPrincipal = tokenHandler.ValidateToken(authToken, validationParameter, out validatedToken);
                }
                catch
                {
                    return false;
                }
            }

            return validatedToken != null;
        }

        private TokenValidationParameters GetTokenValidationParameters(TokenManagementDto tokenManagementDto, bool checkExpiry)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagementDto.Secret));
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = checkExpiry,
                ValidateIssuerSigningKey = true,
                ValidIssuer = tokenManagementDto.Issuer,
                ValidAudience = tokenManagementDto.Audience,
                IssuerSigningKey = key,
            };

        }
    }
}
