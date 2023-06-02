using Microsoft.AspNetCore.Mvc;

namespace ServerPolaris.Controllers
{
    public class ModuloWebController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
