using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    public class RegistroEstudiantesController : Controller
    {
        // GET: RegistroEstudiantes/RegistroEstudiantes
        public IActionResult RegistroEstudiantes()
        {
            return View("~/Views/RegistroEstudiantes/RegistroEstudiantes.cshtml");
        }

        // GET: RegistroEstudiantes/ListaEstudiante
        public IActionResult ListaEstudiante()
        {
            return View("~/Views/ListaEstudiante/ListaEstudiante.cshtml");
        }
    }
}
