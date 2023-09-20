using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    public class IdmProject
    {
        [Column("project_id")]
        public string ProjectId { get; set; }

        [Column("project_name")]
        public string ProjectName { get; set; }

        [Column("project_desc")]
        public string ProjectDesc { get; set; }

        [Column("record_stat")]
        public string RecordStat { get; set; }

        [Column("auth_stat")]
        public string AuthStat { get; set; }

        [Column("checker_id")]
        public string CheckerId { get; set; }

        [Column("checker_dt")]
        public DateTime CheckerDt { get; set; }

        [Column("maker_id")]
        public string MakerId { get; set; }

        [Column("maker_dt")]
        public DateTime MakerDt { get; set; }

        [Column("mod_no")]
        public int ModNo { get; set; }
    }
}
