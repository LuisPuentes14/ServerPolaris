
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
    public class PerfilService : IPerfilService
    {

        private readonly IGenericRepository<Perfil> _repositorio;
        private readonly IPerfilRepository _PerfilRepository;


        public PerfilService(IGenericRepository<Perfil> repositorio, IPerfilRepository perfilRepository)
        {
            _repositorio = repositorio;
            _PerfilRepository = perfilRepository;
        }

        public async Task<List<Perfil>> Lista()
        {
            IQueryable<Perfil> query = await _repositorio.Consultar();
            return query.ToList();
        }

        public async Task<Perfil> Crear(Perfil entidad)
        {
            Perfil perfil_existe = await _repositorio.Obtener(p => p.Descripcion == entidad.Descripcion);

            if (perfil_existe != null)
                throw new TaskCanceledException("El perfil ya existe.");

            try
            {

                Perfil perfil_creado = await _PerfilRepository.Crear(entidad);

                if (perfil_creado.PerfilId == 0)
                    throw new TaskCanceledException("No se pudo crear el perfil");

                IQueryable<Perfil> query = await _repositorio.Consultar(p => p.PerfilId == perfil_creado.PerfilId);

                perfil_creado = query.First();

                return perfil_creado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Perfil> Editar(Perfil entidad)
        {

            try
            {
                IQueryable<Perfil> perfil = await _repositorio.Consultar(p => p.PerfilId == entidad.PerfilId);

                Perfil perfil_para_editar = perfil.First();

                perfil_para_editar.Descripcion = entidad.Descripcion;

                bool respuesta = await _repositorio.Editar(perfil_para_editar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto.");

                Perfil perfil_editado = perfil.First();

                return perfil_editado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int idPerfil)
        {
            try
            {
                Perfil perfil_encontrado = await _repositorio.Obtener(p => p.PerfilId == idPerfil);

                if (perfil_encontrado == null)
                    throw new TaskCanceledException("El producto no existe.");

                bool respuesta = await _PerfilRepository.Eliminar(perfil_encontrado);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
