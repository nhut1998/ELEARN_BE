using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Account.Request
{
    public class RequestRegisterModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]

        public string PhoneNumber { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
