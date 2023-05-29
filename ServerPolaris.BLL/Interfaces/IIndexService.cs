using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IIndexService
    {
        Task<List<ServerPolaris.Entity.Index>> GetInfoIndex();
    }
}
