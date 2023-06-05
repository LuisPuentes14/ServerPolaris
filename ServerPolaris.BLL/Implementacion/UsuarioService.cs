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

        public async Task<List<object>> Lista()
        {
            List<object> query = await _UsuarioRepository.Lista();
            return query;
        }

        public async Task<Usuario> Crear(Usuario entidad)
        {
            Usuario usuario_existe = await _repositorio.Obtener(u => u.UsuEmail == entidad.UsuEmail);

            if (usuario_existe != null)
                throw new TaskCanceledException("El Usuario ya existe.");

            try
            {

                Usuario usuario_creado = await _repositorio.Crear(entidad);

                if (usuario_creado.UsuId == 0)
                    throw new TaskCanceledException("No se pudo crear el cliente");

                IQueryable<Usuario> query = await _repositorio.Consultar(u => u.UsuId == usuario_creado.UsuId);

                usuario_creado = query.First();

                return usuario_creado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Usuario> Editar(Usuario entidad)
        {

            try
            {
                IQueryable<Usuario> queryUsuario = await _repositorio.Consultar(u => u.UsuId == entidad.UsuId);

                Usuario usuario_para_editar = queryUsuario.First();

                usuario_para_editar.UsuNombre = entidad.UsuNombre;
                usuario_para_editar.UsuEmail = entidad.UsuEmail;
                usuario_para_editar.UsuPassword = entidad.UsuPassword;
                usuario_para_editar.EstadoId = entidad.EstadoId;

                bool respuesta = await _repositorio.Editar(usuario_para_editar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto.");

                Usuario usuario_editado = queryUsuario.First();

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
                    throw new TaskCanceledException("El producto no existe.");

                bool respuesta = await _repositorio.Eliminar(usuario_encontrado);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
