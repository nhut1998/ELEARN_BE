using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Fcc.Request
{
    public class RequestTransactionInfoListModel
    {
        [JsonProperty("ccy")]
        public string Ccy { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("drcr")]
        public string Drcr { get; set; }
        [JsonProperty("trn_type")]
        public string TranType { get; set; }
    }
}
