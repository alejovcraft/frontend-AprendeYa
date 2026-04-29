using Frontend_AprendeYa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend_AprendeYa.Controllers
{
    public class VentaController : Controller
    {
        private readonly IVentaService _ventaService;

        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpGet]
        public async Task<IActionResult> ReporteVentas()
        {
            // 1. Verificamos que sea el Administrador (Rol 1)
            var idRol = HttpContext.Session.GetInt32("IdRol");
            if (idRol != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            // 2. Le pedimos los datos crudos a nuestra API
            var reporte = await _ventaService.ObtenerReporteVentasAsync();

            // 3. FÍJATE AQUÍ: El frontend SÍ retorna una View(). 
            // Esto es lo que une tus datos (reporte) con tu HTML (ReporteVentas.cshtml).
            return View(reporte);
        }
    }
}