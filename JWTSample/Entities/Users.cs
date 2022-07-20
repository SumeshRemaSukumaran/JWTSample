using JWTSample.API.Entities;
using JWTSample.Contract.ViewModel;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace JWTSample.Entities
{
    public class Users
    {
        public static List<RefreshToken> RefreshTokens = new List<RefreshToken>();
        public static  List<AuthenticationModel> UserSessions = new List<AuthenticationModel>();

        public static List<UserModel> RegistredUsers = new()
        {
            new UserModel { UserName = "sumesh", Password = "123", GiveName ="Sumesh Sukumaran" }
        };
       
    }
}
