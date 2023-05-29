using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Controllers
{
    public class DataBaseClienteViewController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _DataBaseService;
        private readonly IIndexService _IndexService;



        public DataBaseClienteViewController(IMapper mapper, IDataBaseService DataBaseService)
        {
            _mapper = mapper;
            _DataBaseService = DataBaseService;
        }

     
        public IActionResult Index()
        {            
            return View();
        }

        public async Task<IActionResult> IndexAsync(int idDb)
        {
            List<VMDataBase> vmProductoLista = _mapper.Map<List<VMDataBase>>(await _DataBaseService.Lista());

            //return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
            return View();
        }

        public async Task<IActionResult> ListaMissingIndex()
        {
            List<VMDataBase> vmProductoLista = _mapper.Map<List<VMDataBase>>(await _DataBaseService.Lista());

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
        }


    }
}
