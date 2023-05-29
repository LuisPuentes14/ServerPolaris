using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IDataBaseService
    {
        Task<List<DataBase>> Lista();
        Task<DataBase> Crear(DataBase entidad);
        Task<DataBase> Editar(DataBase entidad);
        Task<bool> Eliminar(int idLog);
    }
}
