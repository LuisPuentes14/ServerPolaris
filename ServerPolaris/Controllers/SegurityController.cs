using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.Models.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace ServerPolaris.Controllers
{
    public class SegurityController : Controller
    {
        public IActionResult Index([FromBody] VMRequestPermisosModulo vMRequestPermisosModulo)
        {

            string modulo = "Clientes/Index";

            GenericResponse<VMPermisosModulo> gResponse = new GenericResponse<VMPermisosModulo>();

            ClaimsPrincipal claimUser = HttpContext.User;

            string permisos = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                //nombreUsuario = claimUser.Claims
                //    .Where(c => c.Type == ClaimTypes.Name)
                //    .Select(c => c.Value).SingleOrDefault();

                permisos = ((ClaimsIdentity)claimUser.Identity).FindFirst("Permisos").Value;

                List<VMPermisosPerfilModulo> listaPermisos = JsonSerializer.Deserialize<List<VMPermisosPerfilModulo>>(permisos);

                VMPermisosModulo vMPermisosModulo = new VMPermisosModulo();

                var aux = listaPermisos.Where(p => p.nombreModulo == modulo && p.PerEliminar == true).ToList();

                if (aux.Count > 0)
                {
                    vMPermisosModulo.PerEliminar = true;
                }

                aux = listaPermisos.Where(p => p.nombreModulo == modulo && p.PerAcceder == true).ToList();

                if (aux.Count > 0)
                {
                    vMPermisosModulo.PerAcceder = true;
                }

                aux = listaPermisos.Where(p => p.nombreModulo == modulo && p.PerInsertar == true).ToList();

                if (aux.Count > 0)
                {
                    vMPermisosModulo.PerInsertar = true;
                }

                aux = listaPermisos.Where(p => p.nombreModulo == modulo && p.PerActualizar == true).ToList();

                if (aux.Count > 0)
                {
                    vMPermisosModulo.PerActualizar = true;
                }

                gResponse.Objeto = vMPermisosModulo;
            }

           








            //List<VMServer> vmLista = _mapper.Map<List<VMServer>>(await _IServerService.GetInfoSever());

            //gResponse.Estado = true;
            //gResponse.ListaObjeto = vmLista;

            return StatusCode(StatusCodes.Status200OK, gResponse);            
        }
    }
}
