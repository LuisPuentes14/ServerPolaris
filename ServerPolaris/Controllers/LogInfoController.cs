using Microsoft.AspNetCore.Mvc;

namespace ServerPolaris.Controllers
{
    public class LogInfoController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index(int idLog, string rutaLog)
        {
            ViewData["id"] = idLog;
            ViewData["ruta"] = rutaLog;
            return View();
        }
    }
}
