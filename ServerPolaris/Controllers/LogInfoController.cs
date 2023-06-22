using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.Models.ViewModels;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;


namespace ServerPolaris.Controllers
{
    [Authorize]
    public class LogInfoController : Controller
    {

        private readonly IMapper _mapper;
        private readonly ILogClienteService _LogClienteServicio;

        public LogInfoController(IMapper mapper, ILogClienteService LogClienteServicio) {

            _mapper = mapper;
            _LogClienteServicio= LogClienteServicio;


        }

        [HttpGet]
        public async Task<IActionResult>  Index(int idLog, string rutaLog)
        {
            VMPermisosModulo vMPermisosModulo =
                ServerPolaris.Utilidades.Security.Security.getPermisos(HttpContext.User,
                $"{ControllerContext.ActionDescriptor.ControllerName}/{ControllerContext.ActionDescriptor.ActionName}");

            if (!vMPermisosModulo.PerAcceder)
            {
                return RedirectToAction("Code403","PolarisServer");
            }


            List<VMLog> vmLogLista = _mapper.Map<List<VMLog>>(await _LogClienteServicio.Lista());

            VMLog vmLog  = vmLogLista.Where(l => l.LogId == idLog).FirstOrDefault();

            ViewBag.Path = vmLog.LogPathFile;
            ViewBag.Cliente = vmLog.ClienteName;
            ViewBag.TypeLog = vmLog.TipoLogDescripcion;
          
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> getFilesPath(string rutaLog)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            string folder = string.Empty;
            string[] part = rutaLog.Split('\\');

            for (int i = 0; i < part.Length - 1; i++)
            {
                folder += $"{part[i]}\\";
            }

            try
            {               
                if (Directory.Exists(folder))
                {                  
                    List<string> archivos = Directory.GetFiles(folder).ToList();
                    gResponse.Estado = true;
                    gResponse.ListaObjeto = archivos;
                }
                else
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "La carpeta no existe.";
                }

            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> DonwloadFile(string rutaLog)
        {
            string rutaArchivo = rutaLog; 
            string[] part = rutaLog.Split('\\');
            string nombreArchivo = part[part.Length - 1];  
            
            if (!System.IO.File.Exists(rutaArchivo))
            {
                return NotFound();
            }           

            byte[] buffer;

            using (FileStream fileStream = System.IO.File.Open(rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=" + nombreArchivo);
            Response.Headers.Add("Content-Length", buffer.Length.ToString());

            return File(buffer, "application/octet-stream");
        }
    }
}
