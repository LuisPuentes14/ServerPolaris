using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Controllers
{
    public class ModuloWebController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IModulosWebService _ModulosWebService;

        public ModuloWebController(IMapper mapper, IModulosWebService ModulosWebService)
        {
            _mapper = mapper;
            _ModulosWebService = ModulosWebService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista(int tipoModulo, long idPadre= 0)
        {
            List<VMModulosWeb> vmProductoLista = _mapper.Map<List<VMModulosWeb>>(await _ModulosWebService.Lista(tipoModulo, idPadre));

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMModulosWeb modelo)
        {
            GenericResponse<VMModulosWeb> gResponse = new GenericResponse<VMModulosWeb>();

            try
            {
                ModulosWeb modulo_creado = await _ModulosWebService.Crear(_mapper.Map<ModulosWeb>(modelo));
                modelo = _mapper.Map<VMModulosWeb>(modulo_creado);

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
        public async Task<IActionResult> Editar([FromBody] VMModulosWeb modelo)
        {
            GenericResponse<VMModulosWeb> gResponse = new GenericResponse<VMModulosWeb>();

            try
            {
                ModulosWeb modulo_editado = await _ModulosWebService.Editar(_mapper.Map<ModulosWeb>(modelo));
                modelo = _mapper.Map<VMModulosWeb>(modulo_editado);

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


    }
}
