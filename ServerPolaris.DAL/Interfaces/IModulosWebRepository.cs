using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Interfaces
{
    public  interface IModulosWebRepository
    {

        Task<ModulosWeb> Crear(ModulosWeb moduloWeb);
        Task<ModulosWeb> Eliminar(ModulosWeb moduloWeb);
    }
}
