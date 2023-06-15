using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Controllers
{
    public class SegurityController : Controller
    {
        public IActionResult Index()
        {

            GenericResponse<VMServer> gResponse = new GenericResponse<VMServer>();
            //List<VMServer> vmLista = _mapper.Map<List<VMServer>>(await _IServerService.GetInfoSever());

            //gResponse.Estado = true;
            //gResponse.ListaObjeto = vmLista;

            return StatusCode(StatusCodes.Status200OK, gResponse);


            
        }
    }
}
