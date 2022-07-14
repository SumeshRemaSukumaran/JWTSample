using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace JWTModels.Dto
{
    [JsonObject("TokenManagement")]
    public class TokenManagementDto
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }
        [JsonProperty("accessExpiration")]
        public string AccessExpiration { get; set; }
        [JsonProperty("refreshExpiraton")]
        public string RefreshExpiraton { get; set; }
    }
}
