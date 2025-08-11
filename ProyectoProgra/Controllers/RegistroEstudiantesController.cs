using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class RegistroEstudiantesController : Controller
    {
        private readonly string _connectionString;

        public RegistroEstudiantesController()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            _connectionString = config.GetConnectionString("EscuelitaDB");
        }

        // GET: RegistroEstudiantes
        [HttpGet]
        public IActionResult RegistroEstudiantes()
        {
            return View("~/Views/RegistroEstudiantes/RegistroEstudiantes.cshtml");
        }

        // POST: RegistroEstudiantes
        [HttpPost]
        public IActionResult RegistroEstudiantes(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Estudiantes (Nombre, Apellido, Edad, Correo) VALUES (@Nombre, @Apellido, @Edad, @Correo)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                        cmd.Parameters.AddWithValue("@Edad", estudiante.Edad);
                        cmd.Parameters.AddWithValue("@Correo", estudiante.Correo);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("ListaEstudiante");
            }

            return View("~/Views/RegistroEstudiantes/RegistroEstudiantes.cshtml", estudiante);
        }

        // GET: Lista de Estudiantes
        public IActionResult ListaEstudiante()
        {
            var estudiantes = new List<Estudiante>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Nombre, Apellido, Edad, Correo FROM Estudiantes";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        estudiantes.Add(new Estudiante
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Edad = reader.GetInt32(3),
                            Correo = reader.GetString(4)
                        });
                    }
                }
            }

            return View("~/Views/ListaEstudiante/ListaEstudiante.cshtml", estudiantes);
        }
    }
}
