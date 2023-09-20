using DecentralizedSystem.API.Models.Account;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DecentralizedSystem.Models.Account
{
    public class UserModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("url_avatar")]
        public string UrlAvatar { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; internal set; }

        [JsonProperty("roles")]
        public List<RoleModel> Roles { get; set; }

        [JsonProperty("department_id")]
        public DepartmentModel Department { get; set; }

        [JsonProperty("branch_id")]
        public BranchModel Branch { get; set; }

    }
}
