namespace ServerPolaris.Models.ViewModels
{
    public class VMDataBase
    {
        public long DataBaseId { get; set; }
        public long ClienteId { get; set; }
        public string DataBaseInstance { get; set; } 
        public string DataBaseName { get; set; } 
        public string DataBaseUser { get; set; } 
        public string DataBasePassword { get; set; }
        public string ClienteName { get; set; }
    }
}
