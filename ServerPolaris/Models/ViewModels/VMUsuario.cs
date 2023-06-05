namespace ServerPolaris.Models.ViewModels
{
    public class VMUsuario
    {
        public long usuLogin { get; set; }

        public string? UsuNombre { get; set; }

        public string? UsuEmail { get; set; }

        public string? UsuLogin { get; set; }

        public string? UsuPassword { get; set; }
        public List<object> Roles { get; set; }         

        public long? EstadoId { get; set; }
    }
}
