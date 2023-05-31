namespace ServerPolaris.Models.ViewModels
{
    public class VMTables
    {
        public string Schema { get; set; }
        public string NameTable { get; set; }
        public long Rows { get; set; }
        public decimal SizeMB { get; set; }
    }
}
