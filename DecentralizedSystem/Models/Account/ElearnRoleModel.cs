using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Account
{
    public class ElearnRoleModel
    {
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        [JsonProperty("role_name")]
        public string RoleName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
