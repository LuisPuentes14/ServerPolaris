using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;

namespace PolarisServer.AplicacionWeb.Controllers
{
    [Authorize]
    public class LogClienteController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogClienteService _LogClienteServicio;

        public LogClienteController(IMapper mapper, ILogClienteService LogClienteServicio)
        {
            _mapper = mapper;
            _LogClienteServicio = LogClienteServicio;
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
            List<VMLog> vmProductoLista = _mapper.Map<List<VMLog>>(await _LogClienteServicio.Lista());

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMLog modelo)
        {
            GenericResponse<VMLog> gResponse = new GenericResponse<VMLog>();

            try
            {

                Log log_creado = await _LogClienteServicio.Crear(_mapper.Map<Log>(modelo));

                modelo = _mapper.Map<VMLog>(log_creado);

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

        public async Task<IActionResult> Editar([FromBody] VMLog modelo)
        {
            GenericResponse<VMLog> gResponse = new GenericResponse<VMLog>();

            try
            {
                Log cliente_editado = await _LogClienteServicio.Editar(_mapper.Map<Log>(modelo));

                modelo = _mapper.Map<VMLog>(cliente_editado);

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
        public async Task<IActionResult> Eliminar(int idLog)
        {

            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _LogClienteServicio.Eliminar(idLog);
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
