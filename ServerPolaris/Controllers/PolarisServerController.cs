using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;
using ServerPolaris.Utilidades.Tools;
using System.Security.Claims;
using System.Text.Json;

namespace ServerPolaris.Controllers
{
    public class PolarisServerController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;
        private readonly IMapper _mapper;

        public PolarisServerController(IUsuarioService usuarioServicio, IMapper mapper)
        {
            _usuarioServicio = usuarioServicio;
            _mapper = mapper;
        }


        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "DashBoard");
                }
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMUsuarioLogin usuario)
        {
            usuario.UsuPassword = Tools.getMD5SHA1(usuario.UsuPassword);


            Usuario usuario_encontrado = await _usuarioServicio.ValidarUsuario(_mapper.Map<Usuario>(usuario));

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<long?> rolesId = new List<long?>();

            foreach (PerfilUsuario perfilUsuario in usuario_encontrado.PerfilUsuarios)
            {
                rolesId.Add(perfilUsuario.PerfilId);
            }

            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name,usuario_encontrado.UsuNombre),
                new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.UsuEmail),
                new Claim(ClaimTypes.Role,JsonSerializer.Serialize(rolesId))

            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); //tipo de autenticación

            AuthenticationProperties properties = new AuthenticationProperties() //propiedades
            {
                AllowRefresh = true, //permite el refrescado de la pagina
                IsPersistent = true, // persiste la sesion y la saca del modelo   
                ExpiresUtc = DateTime.UtcNow.AddMinutes(1),
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties);

            return RedirectToAction("Index", "DashBoard");




        }


    }
}
