using Microsoft.AspNetCore.Mvc;

namespace ServerPolaris.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
