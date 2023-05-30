using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Implementacion;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Models.ViewModels;
using ServerPolaris.Utilidades.Tools;
using System.Text.Json;

namespace ServerPolaris.Controllers
{
    public class DataBaseClienteViewController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _DataBaseService;
        private readonly IIndexService _IndexService;

        public DataBaseClienteViewController(IMapper mapper, IDataBaseService DataBaseService, IIndexService IndexService)
        {
            _mapper = mapper;
            _DataBaseService = DataBaseService;
            _IndexService = IndexService;
        }

     
        public async Task<IActionResult> ViewDataBase(int idDb)
        {
            //Se obtiene la informacion del id de la base de datos
            List<VMDataBase> vmDataBaseLista = _mapper.Map<List<VMDataBase>>(await _DataBaseService.Lista());

            VMDataBase vMDataBase = vmDataBaseLista.Where(d => d.DataBaseId == idDb).FirstOrDefault();

            //Obtenemos la información de conexion a la base de datos
            VMConexion vMConexion = _mapper.Map<VMConexion>(vMDataBase);

            HttpContext.Session.SetString("ServerPolarisSession", JsonSerializer.Serialize(vMConexion));        

          //  var name = HttpContext.Session.GetString("_name");
            //var age = HttpContext.Session.GetInt32("Prueba_2").ToString();

            return View();
        }

        public async Task<IActionResult> getInfoIndex()
        {
            GenericResponse<VMIndex> gResponse = new GenericResponse<VMIndex>();

            var ServerPolarisSession = HttpContext.Session.GetString("ServerPolarisSession");

            string conexion = Tools.getConexion(JsonSerializer.Deserialize<VMConexion>(ServerPolarisSession));

            gResponse.ListaObjeto =  _mapper.Map<List<VMIndex>>(await _IndexService.GetInfoIndex(conexion));


            return StatusCode(StatusCodes.Status200OK, new { data = gResponse.ListaObjeto });
        }




    }
}
