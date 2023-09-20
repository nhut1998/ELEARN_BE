using DecentralizedSystem.Core.Configuration;


namespace DecentralizedSystem.Core.Entities

{
    public class UserManagements
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
        public string PhoneNumber { get; set; }

        [Column("password")]
        public string Password { get; set; }
        
        [Column("role")]
        public string Role { get; set; }


    }
}
