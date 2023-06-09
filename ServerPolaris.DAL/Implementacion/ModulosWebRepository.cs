using Microsoft.EntityFrameworkCore;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Implementacion
{
    public class ModulosWebRepository : IModulosWebRepository
    {

        readonly PolarisServerContext _context;
        public ModulosWebRepository(PolarisServerContext context)
        {
            _context = context;
        }

        public async Task<ModulosWeb> Crear(ModulosWeb moduloWeb)
        {
            ModulosWeb obj = new ModulosWeb();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var idModelo = _context.ModulosWebs.Max(m => m.ModId);


                    if (moduloWeb.IdTipoModulo == 1)
                    {
                        obj.ModIdPadre = idModelo;
                        obj.ModIdHijo = null;
                        obj.ModNombre = moduloWeb.ModNombre;
                        obj.ModUrl = moduloWeb.ModUrl;
                        obj.IdTipoModulo = 1;
                    }
                    else {
                        obj.ModIdPadre = moduloWeb.ModIdPadre;
                        obj.ModIdHijo = idModelo;
                        obj.ModNombre = moduloWeb.ModNombre;
                        obj.ModUrl = moduloWeb.ModUrl;
                        obj.IdTipoModulo = moduloWeb.IdTipoModulo;
                    }

                    _context.ModulosWebs.Add(obj);
                    _context.SaveChanges();

                    var idPerfils = (from perfil in _context.Perfils
                                     select new { perfil.PerfilId }).ToList();


                    foreach (var perfil in idPerfils) { 
                    
                        PermisosPerfilModulo per = new PermisosPerfilModulo();

                        per.PerfilId = perfil.PerfilId;
                        per.ModId = obj.ModId;
                        per.PerAcceder = false;
                        per.PerInsertar = false;
                        per.PerActualizar = false;
                        per.PerEliminar = false;

                        _context.PermisosPerfilModulos.Add(per);
                        _context.SaveChanges();

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

        public async Task<ModulosWeb> Eliminar(ModulosWeb moduloWeb)
        {
            throw new NotImplementedException();
        }
    }
}
