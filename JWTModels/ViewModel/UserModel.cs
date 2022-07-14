using System;
using System.ComponentModel.DataAnnotations;

namespace JWTModels.ViewModel
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Surname { get; set; }
        public string GiveName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
