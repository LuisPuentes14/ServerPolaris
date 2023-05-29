using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.Entity
{
    public class Index
    {
        public string DBName { get; set; }
        public string DatabaseShemaTable { get; set; }
        public string EqualityColumns { get; set; }
        public string IncludedColums { get; set; }
        public string AvgUserImpact { get; set; }
        public string CreateCmd { get; set; }

    }
}
