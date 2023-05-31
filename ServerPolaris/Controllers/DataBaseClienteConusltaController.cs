using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Models.ViewModels;
using ServerPolaris.Utilidades.Tools;
using System.Data;
using System.Text.Json;

namespace ServerPolaris.Controllers
{
    public class DataBaseClienteConusltaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IQueryService _QueryService;
       

        public DataBaseClienteConusltaController(IMapper mapper,IQueryService QueryService)
        {
            _mapper = mapper;
            _QueryService = QueryService;          
        }
        public IActionResult Index()
        {         
            return View();
        }

        public async Task<IActionResult> Query(string comando)
        {

            var ServerPolarisSession = HttpContext.Session.GetString("ServerPolarisSession");

            string conexion = Tools.getConexion(JsonSerializer.Deserialize<VMConexion>(ServerPolarisSession));

            DataTable table = await _QueryService.query(conexion, comando);


            return View(table);
        }

    }
}
