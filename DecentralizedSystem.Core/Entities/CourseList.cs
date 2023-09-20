using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    public class CourseList
    {
        [Column("course_id")]
        public string CourseId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("luotxem")]
        public int Luotxem { get; set; }

        [Column("evaluate")]
        public string Evaluate { get; set; }

        [Column("create_at")]
        public DateTime CreateAt { get; set; }

        [Column("catalog_id")]
        public string CatalogId { get; set; }

        [Column("maker_id")]
        public string MakerId { get; set; }

        [Column("aliases")]
        public string Aliases { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("tuition")]
        public string Tuition { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; } 

        [Column("role")]
        public string Role { get; set; }

        [Column("catalog_name")]
        public string CatalogName { get; set; }

        [Column("catalog_description")]
        public string CatalogDescription { get; set; }

    }
}
