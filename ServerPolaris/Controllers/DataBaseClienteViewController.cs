using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Implementacion;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Models.ViewModels;
using ServerPolaris.Utilidades.Tools;
using System.Collections.Generic;
using System.Text.Json;

namespace ServerPolaris.Controllers
{
    public class DataBaseClienteViewController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _DataBaseService;
        private readonly IIndexService _IndexService;
        private readonly IFilesDataBaseService _FilesDataBaseService;

        public DataBaseClienteViewController(IMapper mapper,
            IDataBaseService DataBaseService,
            IIndexService IndexService,
            IFilesDataBaseService FilesDataBaseService)
        {
            _mapper = mapper;
            _DataBaseService = DataBaseService;
            _IndexService = IndexService;
            _FilesDataBaseService = FilesDataBaseService;
        }


        public async Task<IActionResult> ValidateConexion(int idDb)
        {
                      

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ViewDataBase(int idDb)
        {
            //Se obtiene la informacion del id de la base de datos
            List<VMDataBase> vmDataBaseLista = _mapper.Map<List<VMDataBase>>(await _DataBaseService.Lista());

            VMDataBase vMDataBase = vmDataBaseLista.Where(d => d.DataBaseId == idDb).FirstOrDefault();

            //Obtenemos la información de conexion a la base de datos
            VMConexion vMConexion = _mapper.Map<VMConexion>(vMDataBase);

            HttpContext.Session.SetString("ServerPolarisSession", JsonSerializer.Serialize(vMConexion));                  

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getInfoFiles()
        {        
            var ServerPolarisSession = HttpContext.Session.GetString("ServerPolarisSession");

            string conexion = Tools.getConexion(JsonSerializer.Deserialize<VMConexion>(ServerPolarisSession));

            List <VMFilesDataBase> vMFilesDataBase =  _mapper.Map<List<VMFilesDataBase>>(await _FilesDataBaseService.GetInfoFilesDataBase(conexion));

            return StatusCode(StatusCodes.Status200OK, new { data = vMFilesDataBase });
        }

        [HttpGet]
        public async Task<IActionResult>  getInfoIndex()
        {
            var ServerPolarisSession = HttpContext.Session.GetString("ServerPolarisSession");

            string conexion = Tools.getConexion(JsonSerializer.Deserialize<VMConexion>(ServerPolarisSession));

            List<VMIndex> vMIndices  = _mapper.Map<List< VMIndex >> (await _IndexService.GetInfoIndex(conexion));

            return StatusCode(StatusCodes.Status200OK, new { data = vMIndices });
        }




    }
}
