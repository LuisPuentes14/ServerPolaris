using Microsoft.EntityFrameworkCore;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Implementacion
{
    public class UsuarioRepository: IUsuarioRepository
    {
        readonly PolarisServerContext _context;
        public UsuarioRepository(PolarisServerContext context) {
            _context = context;
        }
        public async Task<List<object>> Lista() {

            List<object> list = new List<object>();

            var query = from ul in _context.Usuarios
                        select new
                        {
                            ul.UsuId,
                            ul.UsuLogin,
                            ul.UsuNombre,
                            ul.UsuEmail,
                            Roles = _context.PerfilUsuarios
                                .Where(pu => pu.UsuId == ul.UsuId)
                                .Join(_context.Perfils, pu => pu.PerfilId, p => p.PerfilId, (pu, p) => new {id = p.PerfilId, descripcion = p.Descripcion } )
                                .ToList(),
                            ul.EstadoId
                        };
;


            foreach (var item in query)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
