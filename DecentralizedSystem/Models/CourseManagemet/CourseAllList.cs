using System;
using System.Collections.Generic;

namespace DecentralizedSystem.Models.CourseManagemet
{
    public class CourseAllListModel
    {
        public string CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Luotxem  { get; set; }
        public string Evaluate { get; set; }
        public DateTime CreateAt { get; set; }
        public string CatalogId { get; set; }
        public string MakerId { get; set; }
        public string Aliases { get; set; }
        public string Status { get; set; }
        public string Tuition { get; set; }
        public MakerModel MakerCourse { get; set; }
        public CatalogListModel CatalogList { get; set; }
        //public int PageNum { get; set; }
        //public int PageSize { get; set; }

    }

    public class MakerModel
    {
        public string Account { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
    }

    public class CatalogListModel
    {
        public string CatalogName { get; set; }
        public string CatalogDescription { get; set; }
    }


}
