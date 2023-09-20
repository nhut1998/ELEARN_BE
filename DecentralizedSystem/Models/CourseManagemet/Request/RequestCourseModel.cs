using Newtonsoft.Json;
using System;
namespace DecentralizedSystem.Models.CourseManagemet.Request
{
    public class RequestCourseModel
    {
      
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("catalog_id")]
        public string CatalogId { get; set; }

        [JsonProperty("maker_id")]
        public string MakerId { get; set; }

        [JsonProperty("aliases")]
        public string Aliases { get; set; }

        [JsonProperty("tuition")]
        public string Tuition { get; set; }

    }
}
