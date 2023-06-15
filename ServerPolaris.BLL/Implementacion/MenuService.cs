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


        public async Task<List<Menu>> Menu(List<long> perfilIds)
        {
            List<Menu> menus = new List<Menu>();

            try
            {
                //List<long> perfilIds = new List<long> { 1, 3 };


                var consulta = (from ppm in _contexto.PermisosPerfilModulos
                                join mw in _contexto.ModulosWebs on ppm.ModId equals mw.ModId
                                join p in _contexto.Perfils on ppm.PerfilId equals p.PerfilId                                
                                where perfilIds.Contains(p.PerfilId) && mw.IdTipoModulo == 1 && ppm.PerAcceder == true
                                select new
                                {
                                    mw.ModId,
                                    mw.ModIdPadre,
                                    mw.ModNombre,
                                    mw.ModUrl,
                                    mw.ModIcono
                                   
                                }).OrderBy(mw => mw.ModId).Distinct().ToList();

                foreach (var item in consulta)
                {
                    Menu menu = new Menu();

                    menu.ModNombre = item.ModNombre;
                    menu.ModIcono = item.ModIcono;
                    menu.ModUrl = item.ModUrl;

                    var consulta2 = (from ppm in _contexto.PermisosPerfilModulos
                                     join sudmodulos in _contexto.ModulosWebs on ppm.ModId equals sudmodulos.ModId
                                     join p in _contexto.Perfils on ppm.PerfilId equals p.PerfilId
                                     where sudmodulos.ModIdPadre == item.ModIdPadre && sudmodulos.IdTipoModulo == 2 && ppm.PerAcceder == true
                                     select new
                                     {
                                         sudmodulos.ModId,
                                         sudmodulos.ModNombre,
                                         sudmodulos.ModUrl,
                                         sudmodulos.ModIcono
                                     }).OrderBy(sudmodulos=> sudmodulos.ModId).Distinct().ToList();

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
