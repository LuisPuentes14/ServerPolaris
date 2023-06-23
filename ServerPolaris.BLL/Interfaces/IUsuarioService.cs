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
        Task<List<UsuarioPerfils>> Lista();
        Task<UsuarioPerfils> Crear(UsuarioPerfils entidad);
        Task<UsuarioPerfils> Editar(UsuarioPerfils entidad);
        Task<bool> Eliminar(int idCliente);
        Task<Usuario> ValidarUsuario(Usuario modelo);
    }
}
