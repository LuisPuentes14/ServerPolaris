
namespace ServerPolaris.Entity
{
    public class CPU
    {
        public int SqlServerCPUUtilization { get; set; }
        public int LdleProcess { get; set; }
        public int OtherCPUUtilization { get; set; }
        public DateTime DateTime { get; set; }        
    }

}
