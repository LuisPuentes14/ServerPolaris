using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioPerfils>> Consultar();
        Task<UsuarioPerfils> Obtener(long idUser);
        Task<UsuarioPerfils> Crear(UsuarioPerfils usuarioPerfil);
        Task<UsuarioPerfils> Editar(UsuarioPerfils usuarioPerfil);
        Task<bool> Eliminar(long idUsuario);
        Task<Usuario> ValidarUsuario(Usuario modelo);
    }
}
