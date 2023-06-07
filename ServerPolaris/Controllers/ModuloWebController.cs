using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.BLL.Interfaces;
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

        
    }
}
