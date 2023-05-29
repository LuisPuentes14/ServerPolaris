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
    public class LogClienteService :  ILogClienteService
    {
        private readonly IGenericRepository<Log> _repositorio;


        public LogClienteService(IGenericRepository<Log> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<Log>> Lista()
        {
            IQueryable<Log> query = await _repositorio.Consultar();                 
            return query.Include(l => l.LogIdTipoLogNavigation).Include(l => l.Cliente).ToList();
        }

        public async Task<Log> Crear(Log entidad)
        {
            Log log_existe = await _repositorio.Obtener(l => l.LogPathFile == entidad.LogPathFile);

            if (log_existe != null)
                throw new TaskCanceledException("El log ya existe.");

            try
            {
                entidad.LogCreateDate = DateTime.Now;
                entidad.LogUpdateDate = null;

                Log log_creado = await _repositorio.Crear(entidad);

                if (log_creado.ClienteId == 0)
                    throw new TaskCanceledException("No se pudo crear el cliente");               

                IQueryable<Log> query = await _repositorio.Consultar(l => l.LogId == log_creado.LogId);

                log_creado = query.Include(l => l.LogIdTipoLogNavigation).Include(l => l.Cliente).First();

                return log_creado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Log> Editar(Log entidad)
        {

            try
            {
                IQueryable<Log> queryLog = await _repositorio.Consultar(l => l.LogId == entidad.LogId);

                Log Log_para_editar = queryLog.First();

                Log_para_editar.ClienteId = entidad.ClienteId;
                Log_para_editar.LogIdTipoLog = entidad.LogIdTipoLog;
                Log_para_editar.LogPathFile = entidad.LogPathFile;

                bool respuesta = await _repositorio.Editar(Log_para_editar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el log.");

                Log log_editado = queryLog.Include(l => l.LogIdTipoLogNavigation).Include(l => l.Cliente).First();

                return log_editado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int idLog)
        {
            try
            {
                Log cliente_encontrado = await _repositorio.Obtener(l => l.LogId == idLog);

                if (cliente_encontrado == null)
                    throw new TaskCanceledException("El log no existe.");

                bool respuesta = await _repositorio.Eliminar(cliente_encontrado);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
