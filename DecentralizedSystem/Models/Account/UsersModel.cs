using DecentralizedSystem.API.Models.Account;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DecentralizedSystem.Models.Account
{
    public class UsersModel
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

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("create_at")]
        public DateTime CreateAt { get; set; }


    }
}
