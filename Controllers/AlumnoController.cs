using Frontend_AprendeYa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend_AprendeYa.Controllers
{
    public class AlumnoController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public AlumnoController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<IActionResult> MisCursos()
        {
            // 1. Verificamos que sea un Alumno (Rol 2)
            var idRol = HttpContext.Session.GetInt32("IdRol");
            if (idRol != 2) return RedirectToAction("Index", "Home");

            // 2. Obtenemos el ID del Usuario desde la sesión
            // (Asegúrate de que en tu AuthService del Frontend estés guardando "IdUsuario" en el SetInt32 de la sesión al hacer login)
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null) return RedirectToAction("Login", "Auth");

            // 3. Pedimos los cursos al Backend
            var misCursos = await _usuarioService.ObtenerCursosDeAlumnoAsync(idUsuario.Value);

            return View(misCursos);
        }
    }
}