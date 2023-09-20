using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Mvc;
namespace DecentralizedSystem.Models.CourseManagemet.Request
{
    public class GetQueryCourseModel
    {

        [JsonProperty("name")]
        [FromQuery(Name = "name")]
        public string Name { get; set; }

        [JsonProperty("catalog_id")]
        [FromQuery(Name = "catalog_id")]
        public string CatalogId { get; set; }

        [JsonProperty("maker_id")]
        [FromQuery(Name = "maker_id")]
        public string MakerId { get; set; }

        [JsonProperty("page_num")]
        [FromQuery(Name = "page_num")]
        public int? PageNum { get; set; }
        [JsonProperty("page_size")]
        [FromQuery(Name = "page_size")]
        public int? PageSize { get; set; }
    }
}
