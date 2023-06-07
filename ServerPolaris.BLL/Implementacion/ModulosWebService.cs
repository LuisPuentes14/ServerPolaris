using Microsoft.EntityFrameworkCore;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Implementacion
{
    public class ModulosWebService: IModulosWebService
    {
        private readonly IGenericRepository<ModulosWeb> _repositorio;


        public ModulosWebService(IGenericRepository<ModulosWeb> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<ModulosWeb>> Lista(int tipoModulo, long idPadre = 0)
        {
            IQueryable<ModulosWeb> query;
            if (idPadre == 0) {
                query = await _repositorio.Consultar(m => m.IdTipoModulo == tipoModulo);
                return query.Include(m => m.IdTipoModuloNavigation).ToList();
            }

            query = await _repositorio.Consultar(m => m.IdTipoModulo == tipoModulo && m.ModIdPadre== idPadre);
            return query.Include(m => m.IdTipoModuloNavigation).ToList();


        }


    }
}
