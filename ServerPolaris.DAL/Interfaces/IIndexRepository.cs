using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Interfaces
{
    public interface IIndexRepository
    {
        Task<List<ServerPolaris.Entity.Index>> GetInfoIndex(string conexion);
    }
}
