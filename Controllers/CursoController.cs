using Frontend_AprendeYa.Models;
using Frontend_AprendeYa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend_AprendeYa.Controllers
{
    public class CursoController : Controller
    {
        private readonly CursoService _cursoService;

        public CursoController(CursoService cursoService)
        {
            _cursoService = cursoService;
        }

        // --- VISTAS PÚBLICAS ---

        public async Task<IActionResult> Index()
        {
            var cursos = await _cursoService.GetCursosAsync();
            return View(cursos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var curso = await _cursoService.GetCursoByIdAsync(id);
            if (curso == null) return NotFound();
            return View(curso);
        }

        //VISTAS DE ADMINISTRADOR (CRUD)

        public async Task<IActionResult> Mantenimiento()
        {
            var rol = HttpContext.Session.GetInt32("IdRol");
            if (rol != 1) return RedirectToAction("Index", "Home");

            var cursos = await _cursoService.GetCursosAsync();
            return View(cursos);
        }

        // CREAR: Muestra el formulario
        public IActionResult Crear()
        {
            var rol = HttpContext.Session.GetInt32("IdRol");
            if (rol != 1) return RedirectToAction("Index", "Home");
            return View();
        }

        // CREAR: Procesa el formulario
        [HttpPost]
        public async Task<IActionResult> Crear(Curso curso)
        {
            var exito = await _cursoService.InsertarAsync(curso);
            if (exito) return RedirectToAction("Mantenimiento");

            ViewBag.Error = "No se pudo crear el curso.";
            return View(curso);
        }

        // EDITAR: Muestra el formulario con datos cargados
        public async Task<IActionResult> Editar(int id)
        {
            var rol = HttpContext.Session.GetInt32("IdRol");
            if (rol != 1) return RedirectToAction("Index", "Home");

            var curso = await _cursoService.GetCursoByIdAsync(id);
            if (curso == null) return NotFound();
            return View(curso);
        }

        // EDITAR: Procesa el cambio
        [HttpPost]
        public async Task<IActionResult> Editar(Curso curso)
        {
            // Nota: Deberás crear el método UpdateAsync en tu CursoService
            // similar al de Insertar pero usando _httpClient.PutAsync
            var exito = await _cursoService.UpdateAsync(curso);
            if (exito) return RedirectToAction("Mantenimiento");

            ViewBag.Error = "No se pudo actualizar el curso.";
            return View(curso);
        }

        // ELIMINAR
        public async Task<IActionResult> Eliminar(int id)
        {
            var rol = HttpContext.Session.GetInt32("IdRol");
            if (rol != 1) return RedirectToAction("Index", "Home");

            // usando _httpClient.DeleteAsync
            await _cursoService.DeleteAsync(id);
            return RedirectToAction("Mantenimiento");
        }
    }
}
