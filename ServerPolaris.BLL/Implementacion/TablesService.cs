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
    public class TablesService : ITablesService
    {
        private readonly ITablesRepository _repositorio;

        public TablesService(ITablesRepository _repositorio)
        {
            this._repositorio = _repositorio;
        }

        public async Task<List<Tables>> GetInfoTables(string conexion)
        {
            List<Tables> listTables = await _repositorio.GetInfoTables(conexion);
            return listTables;
        }


    }
}
