using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    public class Users
    {
        [Column("user_id")]
        public string UserId { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone_number")]
        public string Phonenumber { get; set; }

        [Column("password")]
        public string password { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("maker_at")]
        public DateTime MakerAt { get; set; }


    }
}
