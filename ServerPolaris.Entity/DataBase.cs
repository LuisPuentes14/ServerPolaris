using System;
using System.Collections.Generic;

namespace ServerPolaris.Entity
{
    public partial class DataBase
    {
        public long DataBaseId { get; set; }
        public long ClienteId { get; set; }
        public string DataBaseInstance { get; set; } = null!;
        public string DataBaseName { get; set; } = null!;
        public string DataBaseUser { get; set; } = null!;
        public string DataBasePassword { get; set; } = null!;

        public virtual Cliente Cliente { get; set; } = null!;
    }
}
