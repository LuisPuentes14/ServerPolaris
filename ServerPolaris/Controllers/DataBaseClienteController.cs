using Microsoft.AspNetCore.Mvc;

namespace PolarisServer.AplicacionWeb.Controllers
{
    public class DataBaseClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
