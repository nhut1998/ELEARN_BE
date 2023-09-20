using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DecentralizedSystem.Models.IdmProject.Request
{
    public class AddIdmProjectRequestModel
    {
        [JsonProperty("project_id")]
        [FromQuery(Name = "project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("project_name")]
        [FromQuery(Name = "project_name")]
        public string ProjectName { get; set; }

        [JsonProperty("project_desc")]
        [FromQuery(Name = "project_desc")]
        public string ProjectDesc { get; set; }

        [JsonProperty("record_stat")]
        [FromQuery(Name = "record_stat")]
        public string RecordStat { get; set; }
    }
}
