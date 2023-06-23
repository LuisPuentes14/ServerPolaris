using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.Entity;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PolarisServer.AplicacionWeb.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IClienteService _ClienteServicio;


        public ClientesController(IMapper mapper, IClienteService ClienteServicio)
        {
            _mapper = mapper;
            _ClienteServicio = ClienteServicio;
        }

        public IActionResult Index()
        {            
            VMPermisosModulo vMPermisosModulo =
                ServerPolaris.Utilidades.Security.Security.getPermisos(HttpContext.User, 
                $"{ControllerContext.ActionDescriptor.ControllerName}/{ControllerContext.ActionDescriptor.ActionName}");

            if (!vMPermisosModulo.PerAcceder) {
                return RedirectToAction("Code403","PolarisServer");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {

            List<VMCliente> vmProductoLista = _mapper.Map<List<VMCliente>>(await _ClienteServicio.Lista());

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
        }


        [HttpPost]
        public async Task<IActionResult> Crear( [FromBody] VMCliente modelo)
        {
            GenericResponse<VMCliente> gResponse = new GenericResponse<VMCliente>(); 

            try
            {              

                Cliente cliente_creado = await _ClienteServicio.Crear(_mapper.Map<Cliente>(modelo));

                modelo = _mapper.Map<VMCliente>(cliente_creado);

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

        public async Task<IActionResult> Editar([FromBody] VMCliente modelo)
        {
            GenericResponse<VMCliente> gResponse = new GenericResponse<VMCliente>();

            try
            {             

                Cliente cliente_editado = await _ClienteServicio.Editar(_mapper.Map<Cliente>(modelo));

                modelo = _mapper.Map<VMCliente>(cliente_editado);

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
        public async Task<IActionResult> Eliminar(int idCliente)
        {

            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _ClienteServicio.Eliminar(idCliente);
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
