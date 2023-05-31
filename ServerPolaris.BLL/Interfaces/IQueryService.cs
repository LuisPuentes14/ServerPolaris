using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IQueryService
    {
        Task<DataTable> query(string conexion, string comando);
    }
}
