using JWTSample.API.Entities;
using JWTSample.Contract.Dto;
using JWTSample.Contract.ViewModel;
using System.Threading.Tasks;

namespace JWTSample.Service.Security
{
    public interface IAuthenticationService
    {
        Task<AuthenticationModel> AuthenticateUser(UserLoginDto userLogin);
        Task<bool> LogoutUser(string sessionId);
        Task<AuthenticationModel> GenerateAuthToken(string refreshToken);
        Task<RefreshToken> GenerateRefreshToken(string sessionId);
    }
}
