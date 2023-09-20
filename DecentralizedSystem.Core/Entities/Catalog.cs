using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecentralizedSystem.Core.Configuration;

namespace DecentralizedSystem.Core.Entities
{
    public class Catalog
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("catalog_id")]
        public string CatalogId { get; set; }

        [Column("catalog_name")]
        public string CatalogName { get; set; }

        [Column("icon")]
        public string Icon { get; set; }

        [Column("color")]
        public string Color { get; set; }

        [Column("progress")]
        public int Progress { get; set; }
    }
}
