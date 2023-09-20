using Newtonsoft.Json;
using System;
namespace DecentralizedSystem.Models.CourseManagemet
{
    public class CatalogModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("catalog_id")]
        public string CatalogId { get; set; }

        [JsonProperty("catalog_name")]
        public string CatalogName { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

    }
}
