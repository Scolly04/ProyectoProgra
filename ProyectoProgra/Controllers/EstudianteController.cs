using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    public class EstudianteController : Controller
    {
        // Página principal de estudiantes
        public IActionResult Estudiante()
        {
            return View();
        }

        // Buscar estudiante
        public IActionResult BuscarEstudiante()
        {
            return View();
        }

        // Detalle de un estudiante
        public IActionResult DetalleEstudiante(int id)
        {
            ViewBag.Id = id; // Solo para mostrarlo en la vista
            return View();
        }

        // Editar estudiante
        public IActionResult EditarEstudiante(int id)
        {
            ViewBag.Id = id; // Solo para mostrarlo en la vista
            return View();
        }

        // Eliminar estudiante
        public IActionResult EliminarEstudiante(int id)
        {
            ViewBag.Id = id; // Solo para mostrarlo en la vista
            return View();
        }
    }
}
