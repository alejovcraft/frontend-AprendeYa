using Frontend_AprendeYa.Models;
using Frontend_AprendeYa.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend_AprendeYa.Controllers
{
    public class ModuloController : Controller
    {
        private readonly ModuloService _moduloService;
        private readonly CursoService _cursoService;

        public ModuloController(ModuloService moduloService, CursoService cursoService)
        {
            _moduloService = moduloService;
            _cursoService = cursoService;
        }

        public async Task<IActionResult> Index(int idCurso)
        {
            var curso = await _cursoService.GetCursoByIdAsync(idCurso);
            var modulos = await _moduloService.GetModulosByCursoAsync(idCurso);

            ViewBag.CursoNombre = curso.Titulo;
            ViewBag.IdCurso = idCurso;

            return View(modulos);
        }

        [HttpGet]
        public IActionResult Crear(int idCurso)
        {
            var nuevoModulo = new Modulo { IdCurso = idCurso, };
            return View(nuevoModulo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Modulo modulo)
        {
            var exito = await _moduloService.InsertarAsync(modulo);

            if (exito)
            {                
                return RedirectToAction("Index", new { idCurso = modulo.IdCurso });
            }

            ModelState.AddModelError(string.Empty, "No se pudo crear el módulo. Intente nuevamente.");
            return View(modulo);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var modulo = await _moduloService.GetModuloByIdAsync(id);
            if (modulo == null) return NotFound();

            return View(modulo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Modulo modulo)
        {
            var exito = await _moduloService.UpdateAsync(modulo);
            if (exito)
            {
                return RedirectToAction("Index", new { idCurso = modulo.IdCurso });
            }

            ModelState.AddModelError(string.Empty, "Error al actualizar el módulo.");
            return View(modulo);
        }

       
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var modulo = await _moduloService.GetModuloByIdAsync(id);
            if (modulo == null) return NotFound();

            return View(modulo);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmado(int idModulo, int idCurso)
        {
            var exito = await _moduloService.DeleteAsync(idModulo);
            if (exito)
            {
                return RedirectToAction("Index", new { idCurso = idCurso });
            }

            ViewBag.Error = "No se pudo eliminar el módulo.";
            return RedirectToAction("Index", new { idCurso = idCurso });
        }

    }
}
