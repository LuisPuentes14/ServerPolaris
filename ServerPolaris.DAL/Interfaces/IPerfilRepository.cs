using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Interfaces
{
    public interface IPerfilRepository
    {
        Task<Perfil> Crear(Perfil perfil);
        Task<bool> Eliminar(Perfil perfil);
    }
}
