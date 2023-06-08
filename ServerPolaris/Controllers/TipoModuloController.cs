using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Controllers
{
    public class TipoModuloController : Controller
    {
     
        private readonly IMapper _mapper;
        private readonly ITipoModuloService _TipoModuloService;


        public TipoModuloController(IMapper mapper, ITipoModuloService TipoModuloService)
        {
            _mapper = mapper;
            _TipoModuloService = TipoModuloService;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMTipoModulo> vmProductoLista = _mapper.Map<List<VMTipoModulo>>(await _TipoModuloService.Lista());

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
        }




    }
}
