using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.Entity
{
    public class HardDisk
    {
        public string? Drive { get; set; } 
        public int FreeMB { get; set; }
        public int TotalMB { get; set; }
        public int Free_pct { get; set; }
        public long DbSizeMo { get; set; }
        public int DBGrowthMo { get; set; }
        public int LeftAfterGrowthMo { get; set; }

    }
}
