using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity
{
    public partial class Cliente
    {
        public Cliente()
        {
            Databases = new HashSet<DataBase>();
            Logs = new HashSet<Log>();
        }

        public long ClienteId { get; set; }
        public string ClienteName { get; set; } = null!;

        public virtual ICollection<DataBase> Databases { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
