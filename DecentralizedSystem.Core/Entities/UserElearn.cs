using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    internal class UserElearn
    {
        //[Column("user_id")]
        //public string UserId { get; set; }

        //[Column("user_name")]
        //public string UserName { get; set; }

        //[Column("full_name")]
        //public string FullName { get; set; }

        //[Column("email")]
        //public string Email { get; set; }

        //[Column("phone_number")]
        //public string PhoneNumber { get; set; }

        //[Column("password")]
        //public string Password { get; set; }

        //[Column("status")]
        //public string Status { get; set; }

        //[Column("role")]
        //public string Role { get; set; }

        //[Column("token")]
        //public string Token { get; set; }

        //[Column("marker_at")]
        //public DateTime MakerAt { get; set; }



        [Column("id")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("url_avatar")]
        public string UrlAvatar { get; set; }

        [Column("api_token")]
        public string ApiToken { get; set; }

        [Column("device_token")]
        public string DeviceToken { get; set; }

        [Column("trial_ends_at")]
        public DateTime? TrialEndsAt { get; set; }

        [Column("remember_token")]
        public string RememberToken { get; set; }

        [Column("is_active")]
        public bool IsActive { get; internal set; }

        [Column("created_at")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedDate { get; set; }
    }
}
