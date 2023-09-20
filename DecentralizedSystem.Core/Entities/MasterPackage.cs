using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    public class MasterPackage
    {
        [Column("p_transaction_id")]
        public string PTransactionId { get; set; }

        [Column("p_function_id")]
        public string PFunctionId { get; set; }

        [Column("p_branch_code")]
        public string PBranchCode { get; set; }

        [Column("p_input_details")]
        public string PInputDetails { get; set; }
    }
}
