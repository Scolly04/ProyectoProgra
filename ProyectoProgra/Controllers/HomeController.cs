using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}