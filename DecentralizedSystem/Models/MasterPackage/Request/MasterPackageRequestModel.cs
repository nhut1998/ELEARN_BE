using Newtonsoft.Json;

namespace DecentralizedSystem.Models.MasterPackage.Request
{
    public class MasterPackageRequestModel
    {
        [JsonProperty("p_transaction_id")]
        public string PTransactionId { get; set; }

        [JsonProperty("p_function_id")]
        public string PFunctionId { get; set; }

        [JsonProperty("p_input_details")]
        public object PInputDetails { get; set; }
    }
}
