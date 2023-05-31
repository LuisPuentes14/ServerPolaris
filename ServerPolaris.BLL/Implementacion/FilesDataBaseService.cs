using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Implementacion;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Implementacion
{
    public class FilesDataBaseService : IFilesDataBaseService
    {
        private readonly IFileDataBaseRepository _repositorio;

        public FilesDataBaseService(IFileDataBaseRepository _repositorio)
        {
            this._repositorio = _repositorio;
        }

        public async Task<List<FilesDataBase>> GetInfoFilesDataBase(string conexion)
        {
            List<FilesDataBase> listFiles = await _repositorio.GetInfoFilesDataBase(conexion);
            return listFiles;
        }

    }
}
