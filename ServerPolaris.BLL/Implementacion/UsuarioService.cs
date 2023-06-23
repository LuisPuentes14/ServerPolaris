using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly IUsuarioRepository _UsuarioRepository;

        public UsuarioService(IGenericRepository<Usuario> repositorio, IUsuarioRepository UsuarioRepository)
        {
            _repositorio = repositorio;
            _UsuarioRepository = UsuarioRepository;
        }

        public async Task<List<UsuarioPerfils>> Lista()
        {
            List<UsuarioPerfils> query = await _UsuarioRepository.Consultar();

            return query;
        }

        public async Task<UsuarioPerfils> Crear(UsuarioPerfils entidad)
        {
            Usuario usuario_existe = await _repositorio.Obtener(u => u.UsuEmail == entidad.UsuEmail);

            if (usuario_existe != null)
                throw new TaskCanceledException("El Usuario ya existe.");

            try
            {

                UsuarioPerfils usuario_creado = await _UsuarioRepository.Crear(entidad);

                if (usuario_creado.UsuId == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario");

                UsuarioPerfils query = await _UsuarioRepository.Obtener(usuario_creado.UsuId);

                usuario_creado = query;

                return usuario_creado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UsuarioPerfils> Editar(UsuarioPerfils entidad)
        {

            try
            {
                IQueryable<Usuario> queryUsuario = await _repositorio.Consultar(u => u.UsuId == entidad.UsuId);

                if (queryUsuario == null)
                    throw new TaskCanceledException("El usuario no existe.");


                UsuarioPerfils respuesta = await _UsuarioRepository.Editar(entidad);

                if (respuesta.UsuId == 0)
                    throw new TaskCanceledException("No se pudo editar el producto.");

                UsuarioPerfils usuario_editado = respuesta;

                return usuario_editado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.UsuId == idUsuario);

                if (usuario_encontrado == null)
                    throw new TaskCanceledException("El usuario no existe.");

                bool respuesta = await _UsuarioRepository.Eliminar(idUsuario);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Usuario> ValidarUsuario(Usuario modelo)
        {
            try
            {
                Usuario usuario = new Usuario();

                usuario = await _UsuarioRepository.ValidarUsuario(modelo);
                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
