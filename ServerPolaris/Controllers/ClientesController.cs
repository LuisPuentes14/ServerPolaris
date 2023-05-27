using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServerPolaris.AplicacionWeb.Models.ViewModels;
using ServerPolaris.Entity;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.AplicacionWeb.Utilidades.Response;


namespace PolarisServer.AplicacionWeb.Controllers
{
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
               // VMCliente vmCliente = JsonConvert.DeserializeObject<VMCliente>(modelo);            

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
