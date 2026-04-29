using Frontend_AprendeYa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend_AprendeYa.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        // ==========================================
        // 1. MOSTRAR EL CARRITO (La vista que acabamos de hacer)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Verificamos si el usuario inició sesión
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                TempData["MensajeError"] = "Debes iniciar sesión para ver tu carrito.";
                return RedirectToAction("Login", "Auth"); // Ajusta "Auth" si tu controlador de login se llama distinto
            }

            // Obtenemos el carrito completo desde nuestra API
            var carrito = await _carritoService.ObtenerCarritoAsync(idUsuario.Value);

            // Mandamos los datos a la vista
            return View(carrito);
        }

        // ==========================================
        // 2. AGREGAR UN CURSO AL CARRITO (Desde el botón azul)
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> Agregar(int idCurso, string returnUrl)
        {
            // Verificamos quién está intentando comprar
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            var idRol = HttpContext.Session.GetInt32("IdRol");

            if (idUsuario == null)
            {
                TempData["MensajeError"] = "Debes iniciar sesión para agregar cursos al carrito.";
                return RedirectToAction("Login", "Auth");
            }

            if (idRol != 2) // Solo los alumnos (Rol 2) pueden comprar
            {
                TempData["MensajeError"] = "Solo los estudiantes pueden comprar cursos.";
                return Redirect(returnUrl ?? "/");
            }

            // Llamamos a la API para agregarlo
            var respuesta = await _carritoService.AgregarAlCarritoAsync(idUsuario.Value, idCurso);

            // Configuramos la alerta verde o amarilla según lo que responda el Backend
            if (respuesta.Exito)
                TempData["MensajeExito"] = respuesta.Mensaje;
            else
                TempData["MensajeError"] = respuesta.Mensaje;

            // Devolvemos al usuario a la pantalla en la que estaba (el detalle del curso)
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Curso");
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar(int idDetalle)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null) return RedirectToAction("Login", "Auth");

            var respuesta = await _carritoService.EliminarDelCarritoAsync(idUsuario.Value, idDetalle);

            if (respuesta.Exito)
                TempData["MensajeExito"] = respuesta.Mensaje;
            else
                TempData["MensajeError"] = respuesta.Mensaje;

            return RedirectToAction("Index"); // Recargamos la vista del carrito
        }
        [HttpPost]
        public async Task<IActionResult> Pagar()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null) return RedirectToAction("Login", "Auth");

            var respuesta = await _carritoService.PagarCarritoAsync(idUsuario.Value);

            if (respuesta.Exito)
            {
                TempData["MensajeExito"] = respuesta.Mensaje;
                // Como ya pagó, lo mandamos a su biblioteca para que vea sus cursos
                return RedirectToAction("MisCursos", "Alumno");
            }
            else
            {
                TempData["MensajeError"] = respuesta.Mensaje;
                return RedirectToAction("Index"); // Si falla, lo regresamos al carrito
            }
        }
    }
}