using Newtonsoft.Json;

namespace DecentralizedSystem.Models.Fcc.Request
{
    public class AccountClassFccModel
    {
        [JsonProperty("ma_sp")]
        public string MaSp { get; set; }
        [JsonProperty("ten_sp")]
        public string TenSp { get; set; }
        [JsonProperty("ma_kyhieu")]
        public string MaKyHieu { get; set; }
        [JsonProperty("ma_anchi")]
        public string MaAnChi { get; set; }
    }
}
