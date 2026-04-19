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
            // Llamamos a tu API
            var usuario = await _authService.LoginAsync(request);

            if (usuario != null)
            {
                // Si la API nos devuelve el usuario, guardamos su Token y Nombre en la sesión
                HttpContext.Session.SetString("JWToken", usuario.Token);
                HttpContext.Session.SetString("NombreUsuario", usuario.NombreCompleto);
                HttpContext.Session.SetInt32("IdRol", usuario.IdRol);

                // Lo enviamos a la página principal (Home)
                return RedirectToAction("Index", "Home");
            }

            // Si la API nos rechaza, mostramos un error
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View(request);
        }
    }
}