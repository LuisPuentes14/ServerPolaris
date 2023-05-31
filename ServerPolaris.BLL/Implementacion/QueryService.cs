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
    public class QueryService : IQueryService
    {
        private readonly IQueryRepository _repositorio;


        public QueryService(IQueryRepository repositorio)
        {
            _repositorio = repositorio;
        }
        
        public async Task<DataTable> query(string conexion, string comando)
        {
            DataTable table = await _repositorio.query(conexion, comando);
            return table;
        }


    }
}
