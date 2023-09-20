
using DecentralizedSystem.API.Models.Account;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace DecentralizedSystem.Models.Account
{
    public class RegisterModel
    {
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
