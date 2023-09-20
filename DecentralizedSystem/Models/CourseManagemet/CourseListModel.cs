using Newtonsoft.Json;
using System;
using DecentralizedSystem.Models.Account;
using System.Collections.Generic;

namespace DecentralizedSystem.Models.CourseManagemet
{
    public class CourseListModel
    {

        [JsonProperty("course_id")]
        public string CourseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("luotxem")]
        public int Luotxem { get; set; }

        [JsonProperty("evaluate")]
        public string Evaluate { get; set; }

        [JsonProperty("create_at")]
        public DateTime CreateAt { get; set; }

        [JsonProperty("catalog_id")]
        public string CatalogId { get; set; }

        [JsonProperty("maker_id")]
        public string MakerId { get; set; }

        [JsonProperty("aliases")]
        public string Aliases { get; set; }

        [JsonProperty("staus")]
        public string Status { get; set; }

        [JsonProperty("catalog")]
        public List<ElearnRoleModel> Catalog { get; set; }


    }



}
