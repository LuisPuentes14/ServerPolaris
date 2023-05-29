using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Implementacion
{
    public class IndexService
    {
        private readonly IIndexRepository _repositorio;
        
        public IndexService(IIndexRepository _repositorio)
        {
            this._repositorio = _repositorio;
        }

        public async Task<List<ServerPolaris.Entity.Index>> GetInfoIndex()
        {
            List<ServerPolaris.Entity.Index> listIndex = await _repositorio.GetInfoIndex();
            return listIndex;
        }

    }
}
