using Microsoft.EntityFrameworkCore;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Implementacion
{
    public class MenuService : IMenuService
    {
        
        private readonly PolarisServerContext _contexto;
        public MenuService(IGenericRepository<PermisosPerfilModulo> repositorio, PolarisServerContext contexto)
        {
          
            _contexto = contexto;
        }


        public async Task<List<Menu>> Menu()
        {
            List<Menu> menus = new List<Menu>();

            try
            {
                var consulta = (from ppm in _contexto.PermisosPerfilModulos
                                join mw in _contexto.ModulosWebs on ppm.ModId equals mw.ModId
                                join p in _contexto.Perfils on ppm.PerfilId equals p.PerfilId
                                join pu in _contexto.PerfilUsuarios on p.PerfilId equals pu.PerfilId
                                join u in _contexto.Usuarios on pu.UsuId equals u.UsuId
                                where u.UsuId == 10 && mw.IdTipoModulo == 1
                                select new
                                {
                                    mw.ModIdPadre,
                                    mw.ModNombre,
                                    mw.ModUrl,
                                    mw.ModIcono
                                   
                                }).Distinct().ToList();

                foreach (var item in consulta)
                {
                    Menu menu = new Menu();

                    menu.ModNombre = item.ModNombre;
                    menu.ModIcono = item.ModIcono;
                    menu.ModUrl = item.ModUrl;

                    var consulta2 = (from sudmodulos in _contexto.ModulosWebs
                                     where sudmodulos.ModIdPadre == item.ModIdPadre && sudmodulos.IdTipoModulo == 2
                                     select new
                                     {
                                         sudmodulos.ModNombre,
                                         sudmodulos.ModUrl,
                                         sudmodulos.ModIcono,
                                     }).Distinct().ToList();

                    foreach (var item2 in consulta2)
                    {
                        Menu submenu = new Menu();
                        submenu.ModNombre = item2.ModNombre;
                        submenu.ModIcono = item2.ModIcono;
                        submenu.ModUrl = item2.ModUrl;

                        menu.submodulos.Add(submenu);
                    }

                    menus.Add(menu);
                }




                return menus;

            }
            catch (Exception ex)
            {

                throw ex;
            }
           

        }



    }
}
