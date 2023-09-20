using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Account.Request
{
    public class RequestLoginModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
