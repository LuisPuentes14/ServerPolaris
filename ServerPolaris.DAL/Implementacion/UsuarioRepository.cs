using Microsoft.EntityFrameworkCore;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.DAL.Implementacion
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly PolarisServerContext _context;
        public UsuarioRepository(PolarisServerContext context)
        {
            _context = context;
        }
        public async Task<List<UsuarioPerfils>> Consultar()
        {

            List<UsuarioPerfils> list = new List<UsuarioPerfils>();

            var query = from ul in _context.Usuarios
                        select new
                        {
                            ul.UsuId,
                            ul.UsuLogin,
                            ul.UsuNombre,
                            ul.UsuEmail,
                            Perfils = _context.PerfilUsuarios
                                .Where(pu => pu.UsuId == ul.UsuId)
                                .Join(_context.Perfils, pu => pu.PerfilId, p => p.PerfilId, (pu, p) => new { PerfilId = p.PerfilId, Descripcion = p.Descripcion })
                                .ToList(),
                            ul.EstadoId
                        };


            foreach (var item in query)
            {
                UsuarioPerfils usuarioPerfils = new UsuarioPerfils();

                usuarioPerfils.UsuId = item.UsuId;
                usuarioPerfils.UsuLogin = item.UsuLogin;
                usuarioPerfils.UsuNombre = item.UsuNombre;
                usuarioPerfils.UsuEmail = item.UsuEmail;
                usuarioPerfils.EstadoId = item.EstadoId;

                foreach (var perfils in item.Perfils)
                {
                    usuarioPerfils.Perfils.Add(new Perfil
                    {
                        PerfilId = perfils.PerfilId,
                        Descripcion = perfils.Descripcion,
                    });
                }

                list.Add(usuarioPerfils);
            }
            return list;
        }


        public async Task<UsuarioPerfils> Obtener(long idUser)
        {

            UsuarioPerfils obj = new UsuarioPerfils();

            var query = from ul in _context.Usuarios
                        where ul.UsuId == idUser
                        select new
                        {
                            ul.UsuId,
                            ul.UsuLogin,
                            ul.UsuNombre,
                            ul.UsuEmail,
                            Perfils = _context.PerfilUsuarios
                                .Where(pu => pu.UsuId == ul.UsuId)
                                .Join(_context.Perfils, pu => pu.PerfilId, p => p.PerfilId, (pu, p) => new { PerfilId = p.PerfilId, Descripcion = p.Descripcion })
                                .ToList(),
                            ul.EstadoId
                        };


            foreach (var item in query)
            {
                UsuarioPerfils usuarioPerfils = new UsuarioPerfils();

                usuarioPerfils.UsuId = item.UsuId;
                usuarioPerfils.UsuLogin = item.UsuLogin;
                usuarioPerfils.UsuNombre = item.UsuNombre;
                usuarioPerfils.UsuEmail = item.UsuEmail;
                usuarioPerfils.EstadoId = item.EstadoId;

                foreach (var perfils in item.Perfils)
                {
                    usuarioPerfils.Perfils.Add(new Perfil
                    {
                        PerfilId = perfils.PerfilId,
                        Descripcion = perfils.Descripcion,
                    });
                }

                obj = usuarioPerfils;
            }
            return obj;
        }


        public async Task<UsuarioPerfils> Crear(UsuarioPerfils usuarioPerfil)
        {
            UsuarioPerfils obj = new UsuarioPerfils();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Usuario usuario = new Usuario();
                    usuario.UsuLogin = usuarioPerfil.UsuLogin;
                    usuario.UsuNombre = usuarioPerfil.UsuNombre;
                    usuario.UsuEmail = usuarioPerfil.UsuEmail;
                    usuario.EstadoId = usuarioPerfil.EstadoId;
                    usuario.UsuPassword = usuarioPerfil.UsuPassword;
                    usuario.UsuFechaCreacion = DateTime.Now;
                    usuario.UsuFechaActualizacion = DateTime.Now;
                    usuario.UsuFechaExpPassword = DateTime.Now.AddMonths(3);


                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    foreach (var item in usuarioPerfil.Perfils)
                    {
                        _context.PerfilUsuarios.Add(new PerfilUsuario
                        {
                            PerfilId = item.PerfilId,
                            UsuId = usuario.UsuId
                        });
                        await _context.SaveChangesAsync();
                    }

                    var query = from ul in _context.Usuarios
                                where ul.UsuId == usuario.UsuId
                                select new
                                {
                                    ul.UsuId,
                                    ul.UsuLogin,
                                    ul.UsuNombre,
                                    ul.UsuEmail,
                                    Perfils = _context.PerfilUsuarios
                                        .Where(pu => pu.UsuId == ul.UsuId)
                                        .Join(_context.Perfils, pu => pu.PerfilId, p => p.PerfilId, (pu, p) => new { PerfilId = p.PerfilId, Descripcion = p.Descripcion })
                                        .ToList(),
                                    ul.EstadoId
                                };


                    foreach (var item in query)
                    {
                        UsuarioPerfils usuarioPerfils = new UsuarioPerfils();

                        usuarioPerfils.UsuId = item.UsuId;
                        usuarioPerfils.UsuLogin = item.UsuLogin;
                        usuarioPerfils.UsuNombre = item.UsuNombre;
                        usuarioPerfils.UsuEmail = item.UsuEmail;
                        usuarioPerfils.EstadoId = item.EstadoId;

                        foreach (var perfils in item.Perfils)
                        {
                            usuarioPerfils.Perfils.Add(new Perfil
                            {
                                PerfilId = perfils.PerfilId,
                                Descripcion = perfils.Descripcion,
                            });

                        }

                        obj = usuarioPerfils;
                    }

                    transaction.Commit();

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }

            return obj;
        }

        public async Task<UsuarioPerfils> Editar(UsuarioPerfils usuarioPerfil)
        {
            UsuarioPerfils obj = new UsuarioPerfils();

            Usuario usuario = _context.Usuarios.Where(u => u.UsuId == usuarioPerfil.UsuId).FirstOrDefault();
            if (usuario != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        usuario.UsuLogin = usuarioPerfil.UsuLogin;
                        usuario.UsuNombre = usuarioPerfil.UsuNombre;
                        usuario.UsuEmail = usuarioPerfil.UsuEmail;
                        usuario.EstadoId = usuarioPerfil.EstadoId;
                        //usuario.UsuFechaCreacion = DateTime.Now;
                        usuario.UsuFechaActualizacion = DateTime.Now;
                        //usuario.UsuFechaExpPassword = DateTime.Now.AddMonths(3);

                        if (usuarioPerfil.isUpdatePassword)
                        {
                            usuario.UsuPassword = usuarioPerfil.UsuPassword;
                        }

                        _context.Usuarios.Update(usuario);
                        await _context.SaveChangesAsync();

                        foreach (var item in _context.PerfilUsuarios.Where(pu => pu.UsuId == usuario.UsuId).ToList())
                        {
                            _context.PerfilUsuarios.Remove(item);
                            await _context.SaveChangesAsync();
                        };

                        foreach (var item in usuarioPerfil.Perfils)
                        {
                            _context.PerfilUsuarios.Add(new PerfilUsuario
                            {
                                PerfilId = item.PerfilId,
                                UsuId = usuario.UsuId
                            });
                            await _context.SaveChangesAsync();
                        }

                        var query = from ul in _context.Usuarios
                                    where ul.UsuId == usuario.UsuId
                                    select new
                                    {
                                        ul.UsuId,
                                        ul.UsuLogin,
                                        ul.UsuNombre,
                                        ul.UsuEmail,
                                        Perfils = _context.PerfilUsuarios
                                            .Where(pu => pu.UsuId == ul.UsuId)
                                            .Join(_context.Perfils, pu => pu.PerfilId, p => p.PerfilId, (pu, p) => new { PerfilId = p.PerfilId, Descripcion = p.Descripcion })
                                            .ToList(),
                                        ul.EstadoId
                                    };


                        foreach (var item in query)
                        {
                            UsuarioPerfils usuarioPerfils = new UsuarioPerfils();

                            usuarioPerfils.UsuId = item.UsuId;
                            usuarioPerfils.UsuLogin = item.UsuLogin;
                            usuarioPerfils.UsuNombre = item.UsuNombre;
                            usuarioPerfils.UsuEmail = item.UsuEmail;
                            usuarioPerfils.EstadoId = item.EstadoId;

                            foreach (var perfils in item.Perfils)
                            {
                                usuarioPerfils.Perfils.Add(new Perfil
                                {
                                    PerfilId = perfils.PerfilId,
                                    Descripcion = perfils.Descripcion,
                                });
                            }

                            obj = usuarioPerfils;
                        }

                        transaction.Commit();

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }


                }


            }

            return obj;
        }


        public async Task<bool> Eliminar(long idUsuario)
        {
            bool idDelete = true;

            Usuario usuario = _context.Usuarios.Where(u => u.UsuId == idUsuario).FirstOrDefault();
            if (usuario != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in _context.PerfilUsuarios.Where(pu => pu.UsuId == usuario.UsuId).ToList())
                        {
                            _context.PerfilUsuarios.Remove(item);
                            await _context.SaveChangesAsync();
                        };

                        _context.Usuarios.Remove(usuario);
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        idDelete = false;
                        // throw;
                    }
                }

            }

            return idDelete;
        }

        public async Task<Usuario> ValidarUsuario(Usuario modelo) {

            Usuario usuario = _context.Usuarios.Include(u => u.PerfilUsuarios).FirstOrDefault(u => u.UsuLogin == modelo.UsuLogin && u.UsuPassword == modelo.UsuPassword);

            return usuario;

        }
    }
}
