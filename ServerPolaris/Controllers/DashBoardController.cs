using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Models.ViewModels;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Interfaces;
using AutoMapper;

namespace ServerPolaris.AplicacionWeb.Controllers
{
    //[Authorize]
    public class DashBoardController : Controller
    {   
        private readonly ICPUService _ICPUService;
        private readonly IHardDiskService _IHardDiskService;
        private readonly IRamService _IRamService;
        private readonly IServerService _IServerService;
        private readonly IMapper _mapper;

        public DashBoardController(
            ICPUService ICPUService,
            IHardDiskService IHardDiskService,
            IRamService IRamService,
            IServerService IServerService,
            IMapper mapper)
        {
           
            _ICPUService = ICPUService;
            _IHardDiskService = IHardDiskService;
            _IRamService = IRamService;
            _IServerService = IServerService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetInfoCPU()
        {

            GenericResponse<VMCPU> gResponse = new GenericResponse<VMCPU>();
            List<VMCPU> vmCPULista = _mapper.Map<List<VMCPU>>(await _ICPUService.GetInfoCPU());

            gResponse.Estado = true;
            gResponse.ListaObjeto = vmCPULista;

            return StatusCode(StatusCodes.Status200OK, gResponse);

        }

        [HttpGet]
        public async Task<IActionResult> GetInfoHardDisk()
        {

            GenericResponse<VMHardDisk> gResponse = new GenericResponse<VMHardDisk>();
            List<VMHardDisk> vmLista = _mapper.Map<List<VMHardDisk>>(await _IHardDiskService.GetInfoHardDisk());

            gResponse.Estado = true;
            gResponse.ListaObjeto = vmLista;

            return StatusCode(StatusCodes.Status200OK, gResponse);

        }

        [HttpGet]
        public async Task<IActionResult> GetInfoRam()
        {

            GenericResponse<VMRam> gResponse = new GenericResponse<VMRam>();
            List<VMRam> vmLista = _mapper.Map<List<VMRam>>(await _IRamService.GetInfoRam());

            gResponse.Estado = true;
            gResponse.ListaObjeto = vmLista;

            return StatusCode(StatusCodes.Status200OK, gResponse);

        }

        [HttpGet]
        public async Task<IActionResult> GetInfoServer()
        {

            GenericResponse<VMServer> gResponse = new GenericResponse<VMServer>();
            List<VMServer> vmLista = _mapper.Map<List<VMServer>>(await _IServerService.GetInfoSever());

            gResponse.Estado = true;
            gResponse.ListaObjeto = vmLista;

            return StatusCode(StatusCodes.Status200OK, gResponse);

        }


       
    }
}
