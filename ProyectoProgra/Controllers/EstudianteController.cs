using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly string _connectionString;

        public EstudianteController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexionBD");
        }

        // GET: Lista de estudiantes
        public IActionResult Lista()
        {
            var estudiantes = new List<Estudiante>();

            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                var query = "SELECT Id, Nombre, Apellido, Edad, Correo FROM Estudiante";
                using (var cmd = new SqlCommand(query, con))
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        estudiantes.Add(new Estudiante
                        {
                            Id = dr.GetInt32(0),
                            Nombre = dr.GetString(1),
                            Apellido = dr.GetString(2),
                            Edad = dr.GetInt32(3),
                            Correo = dr.GetString(4)
                        });
                    }
                }
            }
            return View(estudiantes);
        }

        // GET: Formulario de registro
        public IActionResult Registrar()
        {
            return View();
        }

        // POST: Guardar estudiante
        [HttpPost]
        public IActionResult Registrar(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return View(estudiante);

            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                var query = "INSERT INTO Estudiante (Nombre, Apellido, Edad, Correo) VALUES (@Nombre, @Apellido, @Edad, @Correo)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                    cmd.Parameters.AddWithValue("@Edad", estudiante.Edad);
                    cmd.Parameters.AddWithValue("@Correo", estudiante.Correo);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Lista");
        }
    }
}
