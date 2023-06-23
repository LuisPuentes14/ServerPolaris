using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace ServerPolaris.Utilidades.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {

        private readonly IMenuService _menuServicio;
        private readonly IMapper _mapper;

        public MenuViewComponent(IMenuService menuServicio, IMapper mapper)
        {
            _menuServicio = menuServicio;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            ClaimsPrincipal claimUser = HttpContext.User;
            List<VMMenu> listaMenus;

            if (claimUser.Identity.IsAuthenticated)
            {

                string RolUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).SingleOrDefault();

                List<long> listaIds = JsonSerializer.Deserialize<List<long>>(RolUsuario);

                listaMenus = _mapper.Map<List<VMMenu>>(await _menuServicio.Menu(listaIds));
            }
            else
            {
                listaMenus = new List<VMMenu> { };
            }
            return View(listaMenus);
        }


    }

}
