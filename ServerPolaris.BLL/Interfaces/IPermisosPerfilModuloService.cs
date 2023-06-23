using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IPermisosPerfilModuloService
    {

        Task<List<PermisosPerfilModulo>> ObtenerPermisosPerfilModulo(long id);
        Task<List<PermisosPerfilModulo>> Lista();
        Task<bool> Editar(PermisosPerfilModulo permisosPerfilModulo);
    }
}
