using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity
{
    public partial class TipoLog
    {
        public TipoLog()
        {
            Logs = new HashSet<Log>();
        }

        public long TipoLogId { get; set; }
        public string TipoLogDescripcion { get; set; } = null!;

        public virtual ICollection<Log> Logs { get; set; }
    }
}
