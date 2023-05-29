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
    public class DataBaseService: IDataBaseService
    {
        private readonly IGenericRepository<DataBase> _repositorio;


        public DataBaseService(IGenericRepository<DataBase> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<DataBase>> Lista()
        {
            IQueryable<DataBase> query = await _repositorio.Consultar();
            return query.Include(d => d.Cliente).ToList();
        }

        public async Task<DataBase> Crear(DataBase entidad)
        {
            DataBase db_existe = await _repositorio.Obtener(d => d.DataBaseName == entidad.DataBaseName);

            if (db_existe != null)
                throw new TaskCanceledException("La base de datos ya existe.");

            try
            {
                DataBase db_creado = await _repositorio.Crear(entidad);

                if (db_creado.ClienteId == 0)
                    throw new TaskCanceledException("No se pudo crear la base de datos");

                IQueryable<DataBase> query = await _repositorio.Consultar(d => d.DataBaseId == db_creado.DataBaseId);

                db_creado = query.Include(d => d.Cliente).First();

                return db_creado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DataBase> Editar(DataBase entidad)
        {

            try
            {
                IQueryable<DataBase> queryLog = await _repositorio.Consultar(d => d.DataBaseId == entidad.DataBaseId);

                DataBase db_para_editar = queryLog.First();

                db_para_editar.ClienteId = entidad.ClienteId;
                db_para_editar.DataBaseInstance = entidad.DataBaseInstance;
                db_para_editar.DataBaseName = entidad.DataBaseName;
                db_para_editar.DataBaseUser = entidad.DataBaseUser;
                db_para_editar.DataBasePassword = entidad.DataBasePassword;

                bool respuesta = await _repositorio.Editar(db_para_editar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar la base de datos.");

                DataBase log_editado = queryLog.Include(l => l.Cliente).First();

                return log_editado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int idDb)
        {
            try
            {
                DataBase db_encontrado = await _repositorio.Obtener(d => d.DataBaseId == idDb);

                if (db_encontrado == null)
                    throw new TaskCanceledException("La base de datos no existe no existe.");

                bool respuesta = await _repositorio.Eliminar(db_encontrado);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
