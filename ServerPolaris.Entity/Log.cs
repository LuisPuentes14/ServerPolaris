using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity
{
    public partial class Log
    {
        public long LogId { get; set; }
        public long ClienteId { get; set; }
        public long LogIdTipoLog { get; set; }
        public string LogPathFile { get; set; } = null!;
        public DateTime LogCreateDate { get; set; }
        public DateTime LogUpdateDate { get; set; }

        public virtual Cliente Cliente { get; set; } = null!;
        public virtual TipoLog LogIdTipoLogNavigation { get; set; } = null!;
    }
}
