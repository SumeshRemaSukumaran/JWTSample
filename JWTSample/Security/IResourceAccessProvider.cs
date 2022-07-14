using JWTModels.Dto;

namespace JWTSample.Security
{
    public interface IResourceAccessProvider
    {
         bool ValidateToken(string authToken, TokenManagementDto tokenManagementDto, bool checkExpiry);
    }
}
