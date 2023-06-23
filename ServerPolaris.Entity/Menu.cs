using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.Entity
{
    public class Menu
    {

        public string ModNombre { get; set; }
        public string  ModUrl { get; set; }
        public string ModIcono { get; set; }
        public List<Menu> submodulos { get; set; } = new List<Menu>();

    }
}
