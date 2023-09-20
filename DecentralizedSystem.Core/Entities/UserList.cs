using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    public class UserList
    {
        [Column("user_id")]
        public string UserId { get; set; }
        [Column("branch_code")]
        public string BranchCode { get; set; }
        [Column("till_active_flag")]
        public string TillActiveFlag { get; set; }
        [Column("role_id")]
        public string RoleId { get; set; }
    }
}
