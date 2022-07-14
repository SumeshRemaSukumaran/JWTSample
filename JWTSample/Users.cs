using JWTModels.ViewModel;
using System.Collections.Generic;

namespace JWTSample
{
    public class Users
    {
        public static List<UserModel> RegistredUsers = new()
        {
            new UserModel { UserName = "sumesh", Password = "123", GiveName ="Sumesh Sukumaran" }
        };       
    }
}
