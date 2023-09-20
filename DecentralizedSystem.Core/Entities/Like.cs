using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    public class Like
    {
        [Column("course_id")]
        public string CourseID { get; set; }

        [Column("user_id")]
        public string UserID { get; set; }

        [Column("like_at")]
        public DateTime LikeAt { get; set; }
    }
}
