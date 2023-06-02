using ServerPolaris.Entity;
using ServerPolaris.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IPerfilService
    {
        Task<List<Perfil>> Lista();
        Task<Perfil> Crear(Perfil entidad);
        Task<Perfil> Editar(Perfil entidad);
        Task<bool> Eliminar(int idPerfil);

    }
}
