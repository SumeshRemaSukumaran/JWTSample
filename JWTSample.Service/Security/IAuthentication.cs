using JWTSample.Contract.Dto;
using System.Threading.Tasks;

namespace JWTSample.Service.Security
{
    public interface IAuthenticationService
    {
        Task<(bool, string)> AuthenticateUser(UserLoginDto userLogin);
        Task<bool> LogoutUser(string sessionId);
        Task<(bool, string)> GenerateAuthToken();
        Task<(bool, string)> GenerateRefreshToken(string refreshToken);
    }
}
