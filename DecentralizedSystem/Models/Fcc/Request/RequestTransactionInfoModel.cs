using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Fcc.Request
{
    public class RequestTransactionInfoModel
    {
        [JsonProperty("ref_no")]
        public string RefNo { get; set; }
        [JsonProperty("ccy")]
        public string Ccy { get; set; }
        [JsonProperty("loai_gd")]
        public string LoaiGd { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}
