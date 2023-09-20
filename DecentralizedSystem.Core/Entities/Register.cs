using DecentralizedSystem.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentralizedSystem.Core.Entities
{
    public class Register
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
    }
}
