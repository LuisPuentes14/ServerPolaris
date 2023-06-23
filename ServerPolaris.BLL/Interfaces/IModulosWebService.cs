using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IModulosWebService
    {
        Task<List<ModulosWeb>> Lista(int tipoModulo, long idPadre = 0);
        Task<ModulosWeb> Crear(ModulosWeb entidad);
        Task<ModulosWeb> Editar(ModulosWeb entidad);
        Task<bool> Eliminar(ModulosWeb entidad);
    }
}
