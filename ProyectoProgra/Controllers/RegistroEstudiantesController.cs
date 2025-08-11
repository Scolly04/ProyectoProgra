using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class RegistroEstudiantesController : Controller
    {
        private readonly string _connectionString = "Server=localhost;Database=escuelitaDB;Trusted_Connection=True;";

        // GET: Formulario de registro
        [HttpGet]
        public IActionResult RegistroEstudiantes()
        {
            return View();
        }

        // POST: Guardar estudiante
        [HttpPost]
        public IActionResult RegistroEstudiantes(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                return View(estudiante);
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsertarEstudiante", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                cmd.Parameters.AddWithValue("@Edad", estudiante.Edad);
                cmd.Parameters.AddWithValue("@Correo", estudiante.Correo);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["SweetAlertMessage"] = "Estudiante registrado correctamente";
            TempData["SweetAlertIcon"] = "success";
            return RedirectToAction("ListaEstudiantes");
        }

        // GET: Lista de estudiantes
        public IActionResult ListaEstudiantes()
        {
            List<Estudiante> estudiantes = new List<Estudiante>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ListarEstudiantes", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
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

            return View("~/Views/ListaEstudiante/ListaEstudiante.cshtml", estudiantes);
        }

        // GET: Ver detalles
        public IActionResult Detalles(int id)
        {
            var estudiante = ObtenerEstudiantePorId(id);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // GET: Editar estudiante
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var estudiante = ObtenerEstudiantePorId(id);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // POST: Guardar cambios
        [HttpPost]
        public IActionResult Editar(Estudiante estudiante)
        {
            if (!ModelState.IsValid) return View(estudiante);

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ActualizarEstudiante", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", estudiante.Id);
                cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                cmd.Parameters.AddWithValue("@Edad", estudiante.Edad);
                cmd.Parameters.AddWithValue("@Correo", estudiante.Correo);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["SweetAlertMessage"] = "Estudiante actualizado correctamente";
            TempData["SweetAlertIcon"] = "success";
            return RedirectToAction("ListaEstudiantes");
        }

        // POST: Eliminar estudiante
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_EliminarEstudiante", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["SweetAlertMessage"] = "Estudiante eliminado correctamente";
            TempData["SweetAlertIcon"] = "success";
            return RedirectToAction("ListaEstudiantes");
        }

        // Método privado para reutilizar
        private Estudiante ObtenerEstudiantePorId(int id)
        {
            Estudiante estudiante = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ObtenerEstudiantePorId", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
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
            }

            return estudiante;
        }
    }
}
