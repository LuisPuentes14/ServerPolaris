namespace ServerPolaris.Models.ViewModels
{
    public class VMLog
    {
        public long LogId { get; set; }
        public long ClienteId { get; set; }
        public long LogIdTipoLog { get; set; }
        public string LogPathFile { get; set; } = null!;       
        public string ClienteName { get; set; }
        public string TipoLogDescripcion { get; set; }
    }
}
