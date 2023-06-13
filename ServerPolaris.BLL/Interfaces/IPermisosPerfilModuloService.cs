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

        Task<List<PermisosPerfilModulo>> Lista(long id);
        Task<bool> Editar(PermisosPerfilModulo permisosPerfilModulo);
    }
}
