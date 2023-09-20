using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecentralizedSystem
{
    public class PagedResults<T>
    {
        public IReadOnlyList<T>? Result { get; set; }

        public int TotalCount { get; set; }
    }
}
