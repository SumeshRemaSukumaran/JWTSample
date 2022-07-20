using System;

namespace JWTSample.API.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public string SessionId { get; set; }
    }

    public class RefreshTokens
    {

    }
}
