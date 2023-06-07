using ServerPolaris.Entity;

namespace ServerPolaris.Models.ViewModels
{
    public class VMModulosWeb
    {
        public long ModId { get; set; }

        public long? ModIdPadre { get; set; }

        public long? ModIdHijo { get; set; }

        public string? ModNombre { get; set; }

        public string? ModUrl { get; set; }

        public long IdTipoModulo { get; set; }

        public string? DescripcionTipoModulo { get; set; }
    }
}
