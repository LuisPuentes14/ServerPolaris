using ServerPolaris.Entity;

namespace ServerPolaris.Models.ViewModels
{
    public class VMMenu
    {
        public string ModNombre { get; set; }
        public string ModUrl { get; set; }
        public string ModIcono { get; set; }
        public List<VMMenu> submodulos { get; set; } = new List<VMMenu>();
    }
}
