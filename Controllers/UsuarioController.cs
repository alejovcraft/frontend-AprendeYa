using Frontend_AprendeYa.Models;
using Frontend_AprendeYa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend_AprendeYa.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // 1. CARGAR LA PANTALLA PRINCIPAL
        public async Task<IActionResult> Index()
        {
            // Mini-blindaje: Si no es Admin (Rol 1), lo pateamos al Home
            var idRol = HttpContext.Session.GetInt32("IdRol");
            if (idRol != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            // Pedimos los usuarios a la API
            var usuarios = await _usuarioService.ObtenerUsuariosAsync();
            return View(usuarios);
        }

        // 2. GUARDAR (Sirve para Crear y para Editar)
        [HttpPost]
        public async Task<IActionResult> Guardar(UsuarioAdmin usuario)
        {
            // Si el ID es 0, significa que es nuevo
            if (usuario.IdUsuario == 0)
            {
                await _usuarioService.CrearUsuarioAsync(usuario);
            }
            else // Si tiene ID, lo estamos editando
            {
                await _usuarioService.ActualizarUsuarioAsync(usuario);
            }

            // Recargamos la tabla
            return RedirectToAction("Index");
        }

        // 3. ELIMINAR (Baja Lógica)
        [HttpPost]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            await _usuarioService.EliminarUsuarioAsync(idUsuario);
            return RedirectToAction("Index");
        }
        // 4. MATRICULAR ALUMNO
        [HttpPost]
        public async Task<IActionResult> Matricular(int idUsuario, int idCurso)
        {
            var exito = await _usuarioService.MatricularAlumnoAsync(idUsuario, idCurso);

            if (exito)
            {
                TempData["MensajeExito"] = "¡Alumno matriculado correctamente!";
            }
            else
            {
                TempData["MensajeError"] = "Error: El alumno ya tiene este curso o el ID del curso no existe.";
            }

            return RedirectToAction("Index");
        }
    }
}