using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSample.Contract.ViewModel
{
    public class AuthenticationModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public string SessionId { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
