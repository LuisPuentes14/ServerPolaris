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
    public class TipoLogService :ITipoLogService
    {
        private readonly IGenericRepository<TipoLog> _repositorio;


        public TipoLogService(IGenericRepository<TipoLog> repositorio)
        {
            _repositorio = repositorio;
        }

        public Task<TipoLog> Crear(TipoLog entidad)
        {
            throw new NotImplementedException();
        }

        public Task<TipoLog> Editar(TipoLog entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int idCliente)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TipoLog>> Lista()
        {
            IQueryable<TipoLog> query = await _repositorio.Consultar();
            return query.ToList();
        }

    }
}
