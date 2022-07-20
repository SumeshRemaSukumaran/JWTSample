using JWTSample.Contract.Dto;

namespace JWTSample.Service.Security
{
    public interface IResourceAccessProvider
    {
         bool ValidateToken(string authToken, TokenManagementDto tokenManagementDto, bool checkExpiry);
    }
}
