using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            // Ruta absoluta a la vista real
            return View("~/Views/Extras/ReportesEstudiantes.cshtml");
        }
    }
}
