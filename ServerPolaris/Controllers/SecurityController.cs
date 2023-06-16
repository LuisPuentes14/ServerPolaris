﻿using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.Models.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace ServerPolaris.Controllers
{
    public class SecurityController : Controller
    {
        public IActionResult Index([FromBody] VMRequestPermisosModulo vMRequestPermisosModulo)
        {                     

            GenericResponse<VMPermisosModulo> gResponse = new GenericResponse<VMPermisosModulo>();

            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string permisos = "";

                if (claimUser.Identity.IsAuthenticated)
                {
                    List<VMPermisosModulo> ListvMPermisosModulo = new List<VMPermisosModulo>();

                   // vMPermisosModulo = Utilidades.Security.Security.getPermisos(claimUser, vMRequestPermisosModulo.urlModulo);

                    foreach (var otherModulos in vMRequestPermisosModulo.Modulos)
                    {
                        ListvMPermisosModulo.Add(Utilidades.Security.Security.getPermisos(claimUser, otherModulos));
                    }

                    gResponse.Estado = true;
                    gResponse.ListaObjeto = ListvMPermisosModulo;

                }
                else
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "No autenticado...";
                }
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;

            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

    }
}
