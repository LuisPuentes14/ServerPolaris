using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Implementacion;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;
using ServerPolaris.Utilidades.Tools;
using System.Text.Json;

namespace ServerPolaris.Controllers
{
    public class PermisosPerfilModuloController : Controller
    {

        private readonly IPermisosPerfilModuloService _PermisosPerfilModuloService;
        private readonly IPerfilService _PerfilService;
        private readonly IMapper _mapper;

        public PermisosPerfilModuloController(IPermisosPerfilModuloService PermisosPerfilModuloService, IPerfilService PerfilService, IMapper mapper)
        {           
            _PermisosPerfilModuloService = PermisosPerfilModuloService;
            _PerfilService = PerfilService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int id)
        {           
            List<VMPerfil> vmPerfilLista = _mapper.Map<List<VMPerfil>>(await _PerfilService.Lista());

            VMPerfil vMPerfil = vmPerfilLista.Where(d => d.PerfilId == id).FirstOrDefault();           

            HttpContext.Session.SetString("ServerPolarisSession", JsonSerializer.Serialize(vMPerfil));

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var ServerPolarisSession = HttpContext.Session.GetString("ServerPolarisSession");

            VMPerfil perfil = JsonSerializer.Deserialize<VMPerfil>(ServerPolarisSession);

            List<VMPermisosPerfilModulo> vMPermisos = _mapper.Map<List<VMPermisosPerfilModulo>>( await _PermisosPerfilModuloService.ObtenerPermisosPerfilModulo(perfil.PerfilId));

            return StatusCode(StatusCodes.Status200OK, new { data = vMPermisos });
        }


        [HttpPut]
        public async Task<IActionResult> Editar([FromBody]VMPermisosPerfilModulo modelo)
        {

            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                bool cliente_editado = await _PermisosPerfilModuloService.Editar(_mapper.Map<PermisosPerfilModulo>(modelo));
                gResponse.Estado = true;               
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
            
        }


    }
}
