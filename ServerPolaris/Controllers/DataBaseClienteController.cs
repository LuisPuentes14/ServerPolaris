using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;

namespace PolarisServer.AplicacionWeb.Controllers
{
    public class DataBaseClienteController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IDataBaseService _DataBaseService;


        public DataBaseClienteController(IMapper mapper, IDataBaseService DataBaseService)
        {
            _mapper = mapper;
            _DataBaseService = DataBaseService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMDataBase> vmProductoLista = _mapper.Map<List<VMDataBase>>(await _DataBaseService.Lista());

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMDataBase modelo)
        {
            GenericResponse<VMDataBase> gResponse = new GenericResponse<VMDataBase>();

            try
            {
                DataBase db_creado = await _DataBaseService.Crear(_mapper.Map<DataBase>(modelo));

                modelo = _mapper.Map<VMDataBase>(db_creado);

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

        public async Task<IActionResult> Editar([FromBody] VMDataBase modelo)
        {
            GenericResponse<VMDataBase> gResponse = new GenericResponse<VMDataBase>();

            try
            {

                DataBase db_editado = await _DataBaseService.Editar(_mapper.Map<DataBase>(modelo));

                modelo = _mapper.Map<VMDataBase>(db_editado);

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
                gResponse.Estado = await _DataBaseService.Eliminar(idCliente);
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

