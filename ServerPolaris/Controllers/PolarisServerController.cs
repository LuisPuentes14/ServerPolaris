using Microsoft.AspNetCore.Mvc;

namespace ServerPolaris.Controllers
{
    public class PolarisServerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
