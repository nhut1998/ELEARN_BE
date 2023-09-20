namespace DecentralizedSystem.Settings
{
    public class AppConfig
    {
        public string[] DateTimeSmsGenMessage { get; set; }
        public Jwt Jwt { get; set; }
        public ResourceDirectory ResourceDirectory { get; set; }
        public Decentralization Decentralization { get; set; }
        public SoaConfig SoaConfig { get; set; }
        public Accounting Accounting { get; set; }
        public TransactionExcept TransactionExcept { get; set; }
        public TransactionFilter TransactionFilter { get; set; }
    }

    public class Jwt
    {
        public string SecretKey { get; set; }
        public double ExpiryMinutes { get; set; }
        public bool ValidateLifeTime { get; set; }
        public string ValidIssuer { get; set; }
    }

    public class ResourceDirectory
    {
        public string Root { get; set; }
        public string Excel { get; set; }
        public string Word { get; set; }
    }

    public class Decentralization
    {
        public string Url { get; set; }
        public string ProjectId { get; set; }
        public string GroupAdminId { get; set; }
        public string GroupBranchLeaderId { get; set; }
        public string MasterPass => "NHUT_TEST";
        public string MerchantPasswordDefault => "NHUT_TEST";
        public string[] RoleMainTill { get; set; }
        public string[] RoleWithBranch { get; set; }
        public string[] RoleWithAll { get; set; }
        public string[] RoleSealBag { get; set; }
        public string Teller { get; set; }
        public string ExportBill { get; set; }
        public string EnterBill { get; set; }
        public string MainFund { get; set; }
        public string Storekeepers { get; set; }
        public string UHQ { get; set; }
        public string BNP { get; set; }
        public string BK { get; set; }
        public string DMG { get; set; }
        public string XNK { get; set; }
        public string BBGN { get; set; }
        public string CKQ_Rank { get; set; }
  
    }

    public class SoaConfig
    {
        public string Url { get; set; }
        public string Authorization { get; set; }
        public string ClientCode { get; set; }
        public string SecretCode { get; set; }
    }

    public class ScheduleJob
    {
        public string JobOne { get; set; }
    }

    public class Accounting
    {
        public string GLAccountNum { get; set; }
        public string coreTransactionFunctionID { get; set; }
    }
    public class TransactionExcept
    {
        public DrCr DrCr { get; set; }
        public Clearing Clearing { get; set; }
    }
    public class DrCr
    {
        public string[] ByFunc { get; set; }
        public string[] ByRef { get; set; }
    }
    public class Clearing
    {
        public string[] ByFunc { get; set; }
        public string[] ByRef { get; set; }
    }
    public class TransactionFilter
    {
        public PDeliverReceiverStatement PDeliverReceiverStatement { get; set; }
        public string[] PDeliverReceiverByRef { get; set; }
        public string[] AdvanceRefundTreasuryByFunc { get; set; }
    }
    public class PDeliverReceiverStatement
    {
        public string[] ByFunc { get; set; }
        public string[] ByRef { get; set; }
    }
}