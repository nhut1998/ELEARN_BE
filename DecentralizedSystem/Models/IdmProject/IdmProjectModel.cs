using Newtonsoft.Json;
using System;

namespace DecentralizedSystem.Models.IdmProject
{
    public class IdmProjectModel
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("project_name")]
        public string ProjectName { get; set; }

        [JsonProperty("project_desc")]
        public string ProjectDesc { get; set; }

        [JsonProperty("record_stat")]
        public string RecordStat { get; set; }

        [JsonProperty("auth_stat")]
        public string AuthStat { get; set; }

        [JsonProperty("checker_id")]
        public string CheckerId { get; set; }

        [JsonProperty("checker_dt")]
        public DateTime? CheckerDt { get; set; }

        [JsonProperty("maker_id")]
        public string MakerId { get; set; }

        [JsonProperty("maker_dt")]
        public DateTime? MakerDt { get; set; }

        [JsonProperty("mod_no")]
        public int? ModNo { get; set; }
    }
}
