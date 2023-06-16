namespace ServerPolaris.Models.ViewModels
{
    public class VMPermisosModulo
    {
        public bool? PerAcceder { get; set; } = false;
        public bool? PerInsertar { get; set; } = false;
        public bool? PerActualizar { get; set; } = false;
        public bool? PerEliminar { get; set; } = false;
        public string nombreModulo { get; set; } = "";


       // public List<VMPermisosModulo> othersModulos { get; set; } = new List<VMPermisosModulo>();       

    }
}
