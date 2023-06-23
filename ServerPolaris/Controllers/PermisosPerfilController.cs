using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Controllers
{
    [Authorize]
    public class PermisosPerfilController : Controller
    {
        public IActionResult Index()
        {
            VMPermisosModulo vMPermisosModulo =
             ServerPolaris.Utilidades.Security.Security.getPermisos(HttpContext.User,
             $"{ControllerContext.ActionDescriptor.ControllerName}/{ControllerContext.ActionDescriptor.ActionName}");

            if (!vMPermisosModulo.PerAcceder)
            {
                return RedirectToAction("Code403","PolarisServer");
            }

            return View();
        }
    }
}
