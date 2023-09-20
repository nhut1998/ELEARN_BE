using Newtonsoft.Json;
using System;

namespace DecentralizedSystem.Models.UserInfo
{
    public class UserInfoModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("fullname")]
        public string FullName { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("department_id")]
        public string DepartmentId { get; set; }

        [JsonProperty("branch_id")]
        public string BranchId { get; set; }

        [JsonProperty("active_flag")]
        public string ActiveFlag { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("role_fcc")]
        public string RoleFcc { get; set; }

        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName
        {
            get { return $"{UserName} - {FullName}"; }
        }

        [JsonProperty("display_role_name")]
        public string DisplayRoleName
        {
            get { return $"{RoleId} - {UserName} - {FullName}"; }
        }

        [JsonProperty("branch_name")]
        public string BranchName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("is_parent")]
        public int IsParent { get; set; }

        [JsonProperty("parent_branch_code")]
        public string ParentBranchCode { get; set; }

        [JsonProperty("parent_branch_name")]
        public string ParentBranchName { get; set; }
    }
}
