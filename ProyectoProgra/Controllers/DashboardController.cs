using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/UI/Dashboard.cshtml");
        }
    }
}
