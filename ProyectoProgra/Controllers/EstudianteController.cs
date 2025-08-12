using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Lista()
        {
            var estudiantes = new List<Estudiante>();

            try
            {
                using var con = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("SELECT Id, Nombre, Apellido, Edad, Correo FROM Estudiante", con);

                await con.OpenAsync();
                using var dr = await cmd.ExecuteReaderAsync();

                while (await dr.ReadAsync())
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
            catch (SqlException ex)
            {
                // Aquí puedes registrar el error y mostrar una vista de error o mensaje
                ModelState.AddModelError(string.Empty, "Error al cargar los estudiantes: " + ex.Message);
            }

            return View(estudiantes);
        }

        // GET: Formulario para RegistroEstudiantes
        [HttpGet]
        public IActionResult RegistroEstudiantes()
        {
            return View();
        }

        // POST: Registrar estudiante
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return View(estudiante);

            try
            {
                using var con = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("INSERT INTO Estudiante (Nombre, Apellido, Edad, Correo) VALUES (@Nombre, @Apellido, @Edad, @Correo)", con);

                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = estudiante.Nombre;
                cmd.Parameters.Add("@Apellido", SqlDbType.NVarChar, 50).Value = estudiante.Apellido;
                cmd.Parameters.Add("@Edad", SqlDbType.Int).Value = estudiante.Edad;
                cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = estudiante.Correo;

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                TempData["SweetAlertMessage"] = "Estudiante registrado correctamente";
                TempData["SweetAlertIcon"] = "success";

                return RedirectToAction(nameof(Lista));
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar estudiante: " + ex.Message);
                return View(estudiante);
            }
        }

        // GET: Obtener estudiante por ID (asíncrono)
        [HttpGet]
        public async Task<JsonResult> ObtenerEstudiante(int id)
        {
            var estudiante = await ObtenerEstudiantePorIdAsync(id);

            if (estudiante == null)
                return Json(new { success = false, message = "Estudiante no encontrado" });

            return Json(new { success = true, data = estudiante });
        }

        // Método privado asíncrono para obtener estudiante por ID
        private async Task<Estudiante> ObtenerEstudiantePorIdAsync(int id)
        {
            Estudiante estudiante = null;

            try
            {
                using var con = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ObtenerEstudiantePorId", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);

                await con.OpenAsync();
                using var dr = await cmd.ExecuteReaderAsync();

                if (await dr.ReadAsync())
                {
                    estudiante = new Estudiante
                    {
                        Id = dr.GetInt32(0),
                        Nombre = dr.GetString(1),
                        Apellido = dr.GetString(2),
                        Edad = dr.GetInt32(3),
                        Correo = dr.GetString(4)
                    };
                }
            }
            catch (SqlException)
            {
                // Puedes loggear el error si quieres
                estudiante = null;
            }

            return estudiante;
        }
    }
}
