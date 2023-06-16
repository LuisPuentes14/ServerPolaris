using ServerPolaris.Models.ViewModels;
using System.Security.Claims;
using System.Text.Json;


namespace ServerPolaris.Utilidades.Security
{
    public class Security
    {

        public static VMPermisosModulo getPermisos(ClaimsPrincipal claimUser, string modString)
        {
            VMPermisosModulo vMPermisosModulo = new VMPermisosModulo();
            string permisos = "";

            permisos = ((ClaimsIdentity)claimUser.Identity).FindFirst("Permisos").Value;

            List<VMPermisosPerfilModulo> listaPermisos = JsonSerializer.Deserialize<List<VMPermisosPerfilModulo>>(permisos);

            vMPermisosModulo.nombreModulo = modString;


            var aux = listaPermisos.Where(p => p.UrlModulo == modString && p.PerEliminar == true).ToList();

            if (aux.Count > 0)
            {
                vMPermisosModulo.PerEliminar = true;
            }

            aux = listaPermisos.Where(p => p.UrlModulo == modString && p.PerAcceder == true).ToList();

            if (aux.Count > 0)
            {
                vMPermisosModulo.PerAcceder = true;
            }

            aux = listaPermisos.Where(p => p.UrlModulo == modString && p.PerInsertar == true).ToList();

            if (aux.Count > 0)
            {
                vMPermisosModulo.PerInsertar = true;
            }

            aux = listaPermisos.Where(p => p.UrlModulo == modString && p.PerActualizar == true).ToList();

            if (aux.Count > 0)
            {
                vMPermisosModulo.PerActualizar = true;
            }



            return vMPermisosModulo;
        }
    }
}
