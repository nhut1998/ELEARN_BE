using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Fcc
{
    public class UserListModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("branch_code")]
        public string BranchCode { get; set; }
        [JsonProperty("till_active_flag")]
        public string TillActiveFlag { get; set; }
        [JsonProperty("role_id")]
        public string RoleId { get; set; }
    }
}
