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
    public class TipoModuloService: ITipoModuloService
    {
        private readonly IGenericRepository<TipoModulo> _repositorio;


        public TipoModuloService(IGenericRepository<TipoModulo> repositorio)
        {
            _repositorio = repositorio;
        }

        public Task<TipoModulo> Crear(TipoModulo entidad)
        {
            throw new NotImplementedException();
        }

        public Task<TipoModulo> Editar(TipoModulo entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int idTipoModulo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TipoModulo>> Lista()
        {
            IQueryable<TipoModulo> query = await _repositorio.Consultar();
            return query.ToList();
        }


    }
}
