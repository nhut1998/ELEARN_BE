using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    public class UserInfo
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("user_name")]
        public string UserName { get; set; }
        [Column("fullname")]
        public string FullName { get; set; }
        [Column("title")]
        public string Title { get; set; }

        [Column("department_id")]
        public string DepartmentId { get; set; }
        [Column("branch_id")]
        public string BranchId { get; set; }
        [Column("active_flag")]
        public string ActiveFlag { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("role_fcc")]
        public string RoleFcc { get; set; }
        [Column("role_id")]
        public string RoleId { get; set; }
        [Column("branch_name")]
        public string BranchName { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("is_parent")]
        public int IsParent { get; set; }

        [Column("parent_branch_code")]
        public string ParentBranchCode { get; set; }

        [Column("parent_branch_name")]
        public string ParentBranchName { get; set; }

    }
}
