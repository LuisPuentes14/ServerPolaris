using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerPolaris.AplicacionWeb.Utilidades.Response;
using ServerPolaris.BLL.Implementacion;
using ServerPolaris.BLL.Interfaces;
using ServerPolaris.DAL.Interfaces;
using ServerPolaris.Entity;
using ServerPolaris.Models.ViewModels;
using ServerPolaris.Utilidades.Tools;
using System.ComponentModel;

namespace ServerPolaris.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioService _UsarioServicio;
       

        public UsuarioController(IMapper mapper, IUsuarioService UsarioServicio)
        {
            _mapper = mapper;
            _UsarioServicio = UsarioServicio;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<UsuarioPerfils> usuarioLista = await _UsarioServicio.Lista();

            return StatusCode(StatusCodes.Status200OK, new { data = usuarioLista });
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMUsuario modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                modelo.UsuPassword = Tools.getMD5SHA1(modelo.UsuPassword);

                UsuarioPerfils usario_creado = await _UsarioServicio.Crear(_mapper.Map<UsuarioPerfils>(modelo));

                modelo = _mapper.Map<VMUsuario>(usario_creado);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = true;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMUsuario modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                if (modelo.isUpdatePassword) {
                    modelo.UsuPassword = Tools.getMD5SHA1(modelo.UsuPassword);
                }

                UsuarioPerfils usuario_editado = await _UsarioServicio.Editar(_mapper.Map<UsuarioPerfils>(modelo));

                modelo = _mapper.Map<VMUsuario>(usuario_editado);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = true;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }


        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {

            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _UsarioServicio.Eliminar(idUsuario);
            }
            catch (Exception ex)
            {
                gResponse.Estado = true;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);

        }
    }
}
