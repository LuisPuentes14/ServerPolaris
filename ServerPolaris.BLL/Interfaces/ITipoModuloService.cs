using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface ITipoModuloService
    {
        Task<List<TipoModulo>> Lista();
        Task<TipoModulo> Crear(TipoModulo entidad);
        Task<TipoModulo> Editar(TipoModulo entidad);
        Task<bool> Eliminar(int idTipoModulo);


    }
}
