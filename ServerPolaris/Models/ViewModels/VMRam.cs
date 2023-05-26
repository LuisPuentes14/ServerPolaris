namespace ServerPolaris.AplicacionWeb.Models.ViewModels
{
    public class VMRam
    {
        public long PhysicalMemoryMB { get; set; }
        public long AvailableMemoryMB { get; set; }
        public long ToatalPageFileMB { get; set; }
        public long AvailablePageFileMB { get; set; }
        public long SystemCacheMB { get; set; }
        public string? SysteMemoryState { get; set; }
    }
}
