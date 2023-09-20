using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Fcc
{
    public class TransactionInfoListModel
    {
        [JsonProperty("refno")]
        public string RefNo { get; set; }
        [JsonProperty("cmnd")]
        public string Cmnd { get; set; }
        [JsonProperty("custid")]
        public string Custid { get; set; }
        [JsonProperty("functionid")]
        public string FuncitionId { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("ccy")]
        public string Ccy { get; set; }
        [JsonProperty("trndt")]
        public string Trndt { get; set; }
        [JsonProperty("drcrind")]
        public string Drcrind { get; set; }
        [JsonProperty("makerid")]
        public string MakerId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("cmndcif")]
        public string CmndCif { get; set; }
    }
}
