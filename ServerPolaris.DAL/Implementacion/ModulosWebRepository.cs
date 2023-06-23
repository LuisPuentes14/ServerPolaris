using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServerPolaris.DAL.DBContext;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPolaris.DAL.Implementacion
{
    public class ModulosWebRepository : IModulosWebRepository
    {
        private DataTable Table = new DataTable();
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

                    long totalRegistros = _context.ModulosWebs.Count();

                    long idModelo;
                    if (totalRegistros == 0)
                    {
                        idModelo = 1;
                    }
                    else {
                        idModelo = _context.ModulosWebs.Max(m => m.ModId);
                    }                   


                    if (moduloWeb.IdTipoModulo == 1)
                    {
                        obj.ModIdPadre = idModelo;
                        obj.ModIdHijo = null;
                        obj.ModNombre = moduloWeb.ModNombre;
                        obj.ModUrl = moduloWeb.ModUrl;
                        obj.ModIcono = moduloWeb.ModIcono;
                        obj.ModDescripcion = moduloWeb.ModDescripcion;
                        obj.IdTipoModulo = 1;
                    }
                    else {
                        obj.ModIdPadre = moduloWeb.ModIdPadre;
                        obj.ModIdHijo = idModelo;
                        obj.ModNombre = moduloWeb.ModNombre;
                        obj.ModUrl = moduloWeb.ModUrl;
                        obj.ModIcono = moduloWeb.ModIcono;
                        obj.ModDescripcion = moduloWeb.ModDescripcion;
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

                    IQueryable<ModulosWeb>  query = _context.ModulosWebs.Where(m => m.ModId == obj.ModId);
                    query = query.Include(m => m.IdTipoModuloNavigation);

                    obj = query.FirstOrDefault();

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

        public async Task<bool> Eliminar(ModulosWeb moduloWeb)
        {

            try
            {

                Table.Columns.Add("modId", typeof(long));
                Table.Columns.Add("modIdPadre", typeof(string));
                Table.Columns.Add("modIdHijo", typeof(string));
                Table.Columns.Add("modNombre", typeof(string));
                Table.Columns.Add("modUrl", typeof(string));
                Table.Columns.Add("idTipoModulo", typeof(string));
                Table.Columns.Add("modDescripcion", typeof(string));
                Table.Columns.Add("modIcono", typeof(string));

                if (moduloWeb.ModIdHijo == null)
                {
                    recorrerHijos(getHijo(moduloWeb.ModIdPadre));
                }
                else
                {
                    DataRow row = Table.NewRow();
                    row["modId"] = moduloWeb.ModId;
                    row["modIdPadre"] = moduloWeb.ModIdPadre;
                    row["modIdHijo"] = moduloWeb.ModIdHijo;
                    Table.Rows.Add(row);

                    recorrerHijos(getHijo(moduloWeb.ModIdHijo));
                }

               // var table = this.Table;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in this.Table.Rows)
                        {
                            long idMod = Convert.ToInt64(row["modId"]);

                            //var modulosPerfils = _context.PermisosPerfilModulos.Where(p => p.ModId == idMod)

                            foreach (var modulosPerfil in _context.PermisosPerfilModulos.Where(p => p.ModId == idMod).ToList()) {
                                _context.PermisosPerfilModulos.Remove(modulosPerfil);
                                _context.SaveChanges();
                            }                        

                            _context.ModulosWebs.Remove(_context.ModulosWebs.Where(m => m.ModId == idMod).FirstOrDefault());
                            _context.SaveChanges();                           
                           
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();

                        throw;
                    }
                
                }
                   

                return true;

            }
            catch (Exception)
            {

                throw;
            }
            
          

        }


        public void recorrerHijos(List<ModulosWeb> obj)
        {

            foreach (var objecto in obj)
            {
                var hijos = getHijo(objecto.ModIdHijo);

                if (hijos.Count > 0)
                {
                    recorrerHijos(hijos);
                }
            }

        }

        public List<ModulosWeb> getHijo(long? idHijo)
        {

            var query = (_context.ModulosWebs.Where(m => m.ModIdPadre == idHijo)).ToList();

            List<ModulosWeb> modulos = new List<ModulosWeb>();

            foreach (var item in query)
            {

                DataRow row = Table.NewRow();
                row["modId"] = item.ModId;
                row["modIdPadre"] = item.ModIdPadre;
                row["modIdHijo"] = item.ModIdHijo;
               
                Table.Rows.Add(row);

                ModulosWeb obj = new ModulosWeb
                {
                    ModId = item.ModId,
                    ModIdPadre = item.ModIdPadre,
                    ModIdHijo = item.ModIdHijo
                };

                modulos.Add(obj);
            };

            return modulos;

        }
    }
}
