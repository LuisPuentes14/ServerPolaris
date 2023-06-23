using ServerPolaris.Entity;

namespace ServerPolaris.Models.ViewModels
{
    public class VMUsuario
    {
        public long UsuId { get; set; }

        public string? UsuNombre { get; set; }

        public string? UsuEmail { get; set; }

        public string? UsuLogin { get; set; }

        public string? UsuPassword { get; set; } = "";
        public List<Perfil> Perfils { get; set; } = new List<Perfil>();

        public long? EstadoId { get; set; }

        public bool isUpdatePassword { get; set; } = false;
    }
}
