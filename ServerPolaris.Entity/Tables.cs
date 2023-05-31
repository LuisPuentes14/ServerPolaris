using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.Entity
{
    public class Tables
    {
        public string Schema { get; set; }
        public string NameTable { get; set; }
        public long Rows { get; set; }
        public decimal SizeMB { get; set; }

    }
}
