using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DecentralizedSystem.Models.IdmProject.Request
{
    public class GetQueryIdmProjectRequestModel
    {
        [JsonProperty("project_id")]
        [FromQuery(Name = "project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("project_name")]
        [FromQuery(Name = "project_name")]
        public string ProjectName { get; set; }

        [JsonProperty("record_stat")]
        [FromQuery(Name = "record_stat")]
        public string RecordStat { get; set; }

        [JsonProperty("page_num")]
        [FromQuery(Name = "page_num")]
        public int? PageNum { get; set; }

        [JsonProperty("page_size")]
        [FromQuery(Name = "page_size")]
        public int? PageSize { get; set; }
    }
}
