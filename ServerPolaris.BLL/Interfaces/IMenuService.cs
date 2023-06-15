using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.BLL.Interfaces
{
    public interface IMenuService
    {

        Task<List<Menu>> Menu(List<long> perfilIds);
    }
}
