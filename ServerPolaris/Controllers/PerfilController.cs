using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPerfilService _PerfilServicio;
       
        public PerfilController(IMapper mapper, IPerfilService PerfilServicio)
        {
            _mapper = mapper;
            _PerfilServicio = PerfilServicio;
        }

        public IActionResult Index()
        {
            VMPermisosModulo vMPermisosModulo =
                ServerPolaris.Utilidades.Security.Security.getPermisos(HttpContext.User,
                $"{ControllerContext.ActionDescriptor.ControllerName}/{ControllerContext.ActionDescriptor.ActionName}");

            if (!vMPermisosModulo.PerAcceder)
            {
                return RedirectToAction("Code403","PolarisServer");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMPerfil> vmProductoLista = _mapper.Map<List<VMPerfil>>(await _PerfilServicio.Lista());

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMPerfil modelo)
        {
            GenericResponse<VMPerfil> gResponse = new GenericResponse<VMPerfil>();

            try
            {
                Perfil perfil_creado = await _PerfilServicio.Crear(_mapper.Map<Perfil>(modelo));

                modelo = _mapper.Map<VMPerfil>(perfil_creado);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = true;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMPerfil modelo)
        {
            GenericResponse<VMPerfil> gResponse = new GenericResponse<VMPerfil>();

            try
            {
                Perfil Perfil_editado = await _PerfilServicio.Editar(_mapper.Map<Perfil>(modelo));

                modelo = _mapper.Map<VMPerfil>(Perfil_editado);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = true;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idPerfil)
        {

            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _PerfilServicio.Eliminar(idPerfil);
            }
            catch (Exception ex)
            {
                gResponse.Estado = true;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);

        }
    }
}
