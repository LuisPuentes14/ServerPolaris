using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Controllers
{
    [Authorize]
    public class TipoLogController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITipoLogService _TipoLogServicio;


        public TipoLogController(IMapper mapper, ITipoLogService TipoLogServicio)
        {
            _mapper = mapper;
            _TipoLogServicio = TipoLogServicio;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMTipoLog> vmTipoLogLista = _mapper.Map<List<VMTipoLog>>(await _TipoLogServicio.Lista());

            return StatusCode(StatusCodes.Status200OK, new { data = vmTipoLogLista });
        }




    }
}
