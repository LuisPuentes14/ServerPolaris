using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.DBContext
{
    public class PolarisServerStringContext
    {
        public string? Conexion { get; }

        public PolarisServerStringContext(string valor)
        {

            Conexion = valor;
        }
    }
}
