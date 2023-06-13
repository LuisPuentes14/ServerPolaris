using ServerPolaris.Entity;

namespace ServerPolaris.Models.ViewModels
{
    public class VMPermisosPerfilModulo
    {

        public long PerId { get; set; }
        public long? PerfilId { get; set; }
        public long? ModId { get; set; }
        public bool? PerAcceder { get; set; }
        public bool? PerInsertar { get; set; }
        public bool? PerActualizar { get; set; }
        public bool? PerEliminar { get; set; }
        public string nombreModulo { get; set; }
        public string nombrePerfil { get; set; }
        public string tipoModulo { get; set; }
       
    }
}
