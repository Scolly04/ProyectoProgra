using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoProgra.Models
{
    public class DashboardViewModel : Controller
    {
        // GET: DashboardViewModel
        public ActionResult Index()
        {
            return View();
        }

        // GET: DashboardViewModel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardViewModel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardViewModel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardViewModel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardViewModel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardViewModel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardViewModel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
