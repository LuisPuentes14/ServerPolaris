using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface ILogClienteService
    {
        Task<List<Log>> Lista();
        Task<Log> Crear(Log entidad);
        Task<Log> Editar(Log entidad);
        Task<bool> Eliminar(int idLog);
    }
}
