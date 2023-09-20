using Newtonsoft.Json;

namespace DecentralizedSystem.Models
{
    public class DataModel
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
