using Frontend_AprendeYa.Models;
using Frontend_AprendeYa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend_AprendeYa.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ==========================================
        // LOGIN
        // ==========================================

        // GET: Muestra la pantalla de Login vacía
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Se ejecuta cuando el usuario le da al botón "Entrar"
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var usuario = await _authService.LoginAsync(request);

            if (usuario != null)
            {
                // Guardamos los datos en sesión
                HttpContext.Session.SetString("JWToken", usuario.Token);
                HttpContext.Session.SetString("NombreUsuario", usuario.NombreCompleto);
                HttpContext.Session.SetInt32("IdRol", usuario.IdRol);
                HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);


                // LÓGICA DE DIRECCIONAMIENTO POR ROL
                if (usuario.IdRol == 1) // Administrador
                {
                    return RedirectToAction("Mantenimiento", "Curso");
                }
                else if (usuario.IdRol == 2) // Alumno
                {
                    // Lo mandamos al catálogo general por ahora
                    return RedirectToAction("Index", "Curso");
                }
                else if (usuario.IdRol == 3) // Profesor
                {
                    // Lo mandamos a la vista que crearemos después
                    return RedirectToAction("MisClases", "Profesor");
                }

                // Por si acaso entra un rol desconocido
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View(request);
        }

        // ==========================================
        // CERRAR SESIÓN (LOGOUT)
        // ==========================================
        [HttpGet]
        public IActionResult Logout()
        {
            // 1. Borramos absolutamente todo de la sesión (Token, IdRol, NombreUsuario)
            HttpContext.Session.Clear();

            // 2. Te devolvemos al Index genérico del Home, como un visitante anónimo
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // REGISTRO
        // ==========================================

        // GET: Muestra la pantalla de Registro vacía
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: Se ejecuta cuando el usuario hace clic en "Registrarse"
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroRequest request)
        {
            var exito = await _authService.RegistrarAsync(request);

            if (exito)
            {
                // Si todo sale bien, mensaje verde y lo enviamos al Login
                TempData["Exito"] = "¡Cuenta creada con éxito! Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            // Si falla (ej. correo duplicado o error de la API)
            ViewBag.Error = "No se pudo crear la cuenta. Es posible que el Usuario o Correo ya existan.";
            return View(request);
        }
    }
}