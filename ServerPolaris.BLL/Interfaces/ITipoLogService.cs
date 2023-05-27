using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface ITipoLogService
    {
        Task<List<TipoLog>> Lista();
        Task<TipoLog> Crear(TipoLog entidad);
        Task<TipoLog> Editar(TipoLog entidad);
        Task<bool> Eliminar(int idCliente);
    }
}
