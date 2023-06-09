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

        public async Task<ModulosWeb> Eliminar(ModulosWeb moduloWeb)
        {
            throw new NotImplementedException();
        }



        //public void recorrerHijos(List<ModulosWeb> obj)
        //{

        //    foreach (var objecto in obj)
        //    {
        //        var hijos = getHijo(objecto.id_documento_hijo);

        //        if (hijos.Count > 0)
        //        {
        //            recorrerHijos(hijos);
        //        }
        //    }

        //}

        //public List<ModulosWeb> getHijo(Int64 x)
        //{

        //    var query = (from documentoXcliente in _contextData.documentoXcliente
        //                 where (documentoXcliente.id_documento_padre == x
        //                 && documentoXcliente.id_cliente == cliente)
        //                 select new
        //                 {
        //                     documentoXcliente.id_documento_padre,
        //                     documentoXcliente.nombre_documento_padre,
        //                     documentoXcliente.id_documento_hijo,
        //                     documentoXcliente.nombre_documento_hijo
        //                 }).ToList();

        //    List<CE_DocumentoXcliente> cE_DocumentoXcliente = new List<CE_DocumentoXcliente>();

        //    foreach (var item in query)
        //    {

        //        DataRow row = Table.NewRow();
        //        row["id_documento_padre"] = item.id_documento_padre;
        //        row["nombre_documento_padre"] = item.nombre_documento_padre;
        //        row["id_documento_hijo"] = item.id_documento_hijo;
        //        row["nombre_documento_hijo"] = item.nombre_documento_hijo;
        //        Table.Rows.Add(row);

        //        CE_DocumentoXcliente obj = new CE_DocumentoXcliente
        //        {
        //            id_documento_padre = item.id_documento_padre,
        //            nombre_documento_padre = item.nombre_documento_padre,
        //            id_documento_hijo = item.id_documento_hijo,
        //            nombre_documento_hijo = item.nombre_documento_hijo


        //        };

        //        cE_DocumentoXcliente.Add(obj);
        //    };

        //    return cE_DocumentoXcliente;

        //}
    }
}
