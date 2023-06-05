using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<object>> Lista();
        Task<Usuario> Crear(Usuario entidad);
        Task<Usuario> Editar(Usuario entidad);
        Task<bool> Eliminar(int idCliente);
    }
}
