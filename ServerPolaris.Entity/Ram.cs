using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.Entity
{
    public class Ram
    {
        public long PhysicalMemoryMB { get; set; }
        public long AvailableMemoryMB { get; set; }
        public long ToatalPageFileMB { get; set; }
        public long AvailablePageFileMB { get; set; }
        public long SystemCacheMB { get; set; }
        public string? SysteMemoryState  { get; set; }
        
    }
}
