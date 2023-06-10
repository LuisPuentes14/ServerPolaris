
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;


namespace ServerPolaris.DAL.Implementacion
{
    public class PerfilRepository: IPerfilRepository
    {
        readonly PolarisServerContext _context;
        public PerfilRepository(PolarisServerContext context)
        {
            _context = context;
        }
        public async Task<Perfil> Crear(Perfil perfil)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Perfils.Add(perfil);
                    _context.SaveChanges();

                    var listMod = _context.ModulosWebs.ToList();


                    foreach (var mod in listMod)
                    {
                        PermisosPerfilModulo permisosPerfilModulo = new PermisosPerfilModulo();

                        permisosPerfilModulo.PerfilId = perfil.PerfilId;
                        permisosPerfilModulo.ModId = mod.ModId;
                        permisosPerfilModulo.PerAcceder = false;
                        permisosPerfilModulo.PerInsertar = false;
                        permisosPerfilModulo.PerActualizar = false;
                        permisosPerfilModulo.PerEliminar = false;


                        _context.PermisosPerfilModulos.Add(permisosPerfilModulo);
                        _context.SaveChanges();

                    }

                    transaction.Commit();

                    return perfil;

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }

        }

        public async Task<bool> Eliminar(Perfil perfil)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var listMod = _context.PermisosPerfilModulos.Where(p => p.PerfilId == perfil.PerfilId).ToList();

                    foreach (var mod in listMod)
                    {
                        _context.PermisosPerfilModulos.Remove(mod);
                        _context.SaveChanges();

                    }

                    _context.Perfils.Remove(perfil);
                    _context.SaveChanges();

                    transaction.Commit();

                    return true;

                }
                catch (Exception)
                {
                    transaction.Rollback();

                    throw;
                }

            }



        }



    }
}
