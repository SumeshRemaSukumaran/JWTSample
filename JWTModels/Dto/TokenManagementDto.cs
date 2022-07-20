using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace JWTSample.Contract.Dto
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
        [JsonProperty("accessExpirationMinutes")]
        public string AccessExpirationMinutes { get; set; }
        [JsonProperty("refreshExpiratonMinutes")]
        public string RefreshExpiratonMinutes { get; set; }
    }
}
