using Microsoft.AspNetCore.Mvc;

namespace PolarisServer.AplicacionWeb.Controllers
{
    public class LogCliente : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
