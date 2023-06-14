using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;
using System.Security.Claims;

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

            //if (claimUser.Identity.IsAuthenticated)
            //{

            //    string idUsuario = claimUser.Claims
            //        .Where(c => c.Type == ClaimTypes.NameIdentifier)
            //    .Select(c => c.Value).SingleOrDefault();

            listaMenus = _mapper.Map<List<VMMenu>>(await _menuServicio.Menu());
            //}
            //else
            //{
            //    listaMenus = new List<VMMenu> { };
            //}
            return View(listaMenus);
        }


    }

}
