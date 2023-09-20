using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Fcc.Request
{
    public class RequestUserListModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
