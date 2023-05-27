

namespace ServerPolaris.Models.ViewModels
{
    public class VMCPU
    {        
        public int SqlServerCPUUtilization { get; set; }
        public int LdleProcess { get; set; }
        public int OtherCPUUtilization { get; set; }
        public DateTime DateTime { get; set; }
    }
}
