using JWTModels.Dto;
using System.Threading.Tasks;

namespace JWTSample.Security
{
    public interface IAuthentication
    {
        Task<(bool, string)> AuthenticateUser(UserLoginDto userLogin);
        Task<bool> LogoutUser(string sessionId);
        Task<(bool, string)> GenerateAuthToken();
        Task<(bool, string)> GenerateRefreshToken(string refreshToken);
    }
}
