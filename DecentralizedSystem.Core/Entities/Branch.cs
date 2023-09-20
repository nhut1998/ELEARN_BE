using DecentralizedSystem.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentralizedSystem.Core.Entities
{
    public class Branch
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("parent_id")]
        public string ParentId { get; set; }

        [Column("parent_code")]
        public string ParentCode { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("area")]
        public string Area { get; set; }

        [Column("Rank")]
        public int Rank { get; set; }

        [Column("modify_id")]
        public string ModifyId { get; set; }

        [Column("modify_at")]
        public DateTime? ModifyAt { get; set; }

        [Column("is_warehouse")]
        public int IsWarehouse { get; set; }

        [Column("is_qp")]
        public int IsQp { get; set; }

        [Column("active_flag")]
        public int ActiveFlag { get; set; }

        [Column("created_at")]
        public DateTime CreatedDate { get; set; }

        [Column("status_tran_lock")]
        public string StatusTranLock { get; set; }

        [Column("tran_date")]
        public DateTime? TranDate { get; set; }
    }
}
