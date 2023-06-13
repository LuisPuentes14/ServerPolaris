using Microsoft.EntityFrameworkCore;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Implementacion
{
    public class PermisosPerfilModuloService : IPermisosPerfilModuloService
    {

        private readonly IGenericRepository<PermisosPerfilModulo> _repositorio;


        public PermisosPerfilModuloService(IGenericRepository<PermisosPerfilModulo> repositorio)
        {
            _repositorio = repositorio;
        }


        public async Task<List<PermisosPerfilModulo>> Lista(long id)
        {
            IQueryable<PermisosPerfilModulo> query = await _repositorio.Consultar( p => p.PerfilId == id);
            return query.Include(p => p.Mod).Include(p => p.Perfil).Include(p => p.Mod.IdTipoModuloNavigation).ToList();
        }


        public async Task<bool> Editar(PermisosPerfilModulo permisosPerfilModulo)
        {

            bool isEdit = false;
            PermisosPerfilModulo permisos_editar = await _repositorio.Obtener(p => p.PerId == permisosPerfilModulo.PerId);

            if (permisos_editar == null) {
                throw new TaskCanceledException("No existen permisos.");
            }

            permisos_editar.PerEliminar = permisosPerfilModulo.PerEliminar;
            permisos_editar.PerActualizar = permisosPerfilModulo.PerActualizar;
            permisos_editar.PerInsertar = permisosPerfilModulo.PerInsertar;
            permisos_editar.PerAcceder = permisosPerfilModulo.PerAcceder;

            isEdit = await _repositorio.Editar(permisos_editar);
            
            return isEdit;
        }



    }
}
