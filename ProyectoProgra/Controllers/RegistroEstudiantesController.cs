using Microsoft.AspNetCore.Mvc;
using TuProyecto.Models;
using System.Collections.Generic;

namespace TuProyecto.Controllers
{
    public class RegistroEstudiantesController : Controller
    {
        // Lista simulando una base de datos (puedes reemplazar por EF Core)
        private static List<Estudiante> estudiantes = new List<Estudiante>();

        public IActionResult RegistroEstudiantes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistroEstudiantes(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                estudiante.Id = estudiantes.Count + 1;
                estudiantes.Add(estudiante);
                return RedirectToAction("ListaEstudiante", "RegistroEstudiantes");
            }
            return View(estudiante);
        }

        public IActionResult ListaEstudiante()
        {
            return View(estudiantes);
        }
    }
}