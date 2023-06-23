using Microsoft.EntityFrameworkCore;
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
    public class ModulosWebService: IModulosWebService
    {
        private readonly IGenericRepository<ModulosWeb> _repositorio;
        private readonly IModulosWebRepository _modulosWebRepository;


        public ModulosWebService(IGenericRepository<ModulosWeb> repositorio, IModulosWebRepository modulosWebRepository)
        {
            _repositorio = repositorio;
            _modulosWebRepository = modulosWebRepository;
        }

        public async Task<List<ModulosWeb>> Lista(int tipoModulo, long idPadre = 0)
        {
            IQueryable<ModulosWeb> query;
            if (idPadre == 0)
            {
                query = await _repositorio.Consultar(m => m.IdTipoModulo == tipoModulo);
                return query.Include(m => m.IdTipoModuloNavigation).ToList();
            }

            query = await _repositorio.Consultar(m => m.IdTipoModulo == tipoModulo && m.ModIdPadre == idPadre);
            return query.Include(m => m.IdTipoModuloNavigation).ToList();

        }


        public async Task<ModulosWeb> Crear(ModulosWeb entidad)
        {
            ModulosWeb queryModulo = await _repositorio.Obtener(c => c.ModNombre == entidad.ModNombre && c.ModUrl == entidad.ModUrl);

            if (queryModulo != null) {
                throw new NotImplementedException("El modulo ya existe");
            }

            ModulosWeb modulo_creado = await _modulosWebRepository.Crear(entidad);

            return modulo_creado;
        }

        public async Task<ModulosWeb> Editar(ModulosWeb entidad)
        {
            try
            {
                IQueryable<ModulosWeb> queryModulo = await _repositorio.Consultar(c => c.ModId == entidad.ModId);

                ModulosWeb Modulo_para_editar = queryModulo.First();

                Modulo_para_editar.ModNombre = entidad.ModNombre;
                Modulo_para_editar.ModUrl = entidad.ModUrl;
                Modulo_para_editar.ModDescripcion = entidad.ModDescripcion;
                Modulo_para_editar.ModIcono = entidad.ModIcono;

                bool respuesta = await _repositorio.Editar(Modulo_para_editar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto.");

                ModulosWeb producto_editado = queryModulo.Include(m => m.IdTipoModuloNavigation).First();

                return producto_editado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(ModulosWeb entidad)
        {
            bool isDelte  = false;

            ModulosWeb queryModulo = await _repositorio.Obtener(c => c.ModId == entidad.ModId );

            if (queryModulo == null)
            {
                throw new NotImplementedException("El modulo no existe");
            }


            isDelte = await _modulosWebRepository.Eliminar(entidad);

            return isDelte;
        }

        


    }
}
