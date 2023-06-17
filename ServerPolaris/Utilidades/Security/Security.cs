using ServerPolaris.Models.ViewModels;
using System.Reflection.Metadata.Ecma335;
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

            List<VMPermisosPerfilModulo> aux = listaPermisos.Where(p => p.UrlModulo == modString).ToList();

            foreach (var item in aux) {
                vMPermisosModulo.PerActualizar = (bool) item.PerActualizar ?  true : vMPermisosModulo.PerActualizar;
                vMPermisosModulo.PerEliminar = (bool) item.PerEliminar  ? true : vMPermisosModulo.PerEliminar;
                vMPermisosModulo.PerInsertar = (bool) item.PerInsertar  ? true : vMPermisosModulo.PerInsertar;
                vMPermisosModulo.PerAcceder = (bool) item.PerAcceder ? true : vMPermisosModulo.PerAcceder;
            }        

            return vMPermisosModulo;
        }
    }
}
