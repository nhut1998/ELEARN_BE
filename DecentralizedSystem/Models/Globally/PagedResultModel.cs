using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecentralizedSystem.Models.Globally
{
    public class PagedResultModel<T>
    {
        [JsonProperty("result")]
        public IEnumerable<T> Result { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}
