using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using TuProyecto.Models;
using Microsoft.Extensions.Configuration;

namespace TuProyecto.Controllers
{
    public class RegistroEstudiantesController : Controller
    {
        private readonly string _connectionString;

        public RegistroEstudiantesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexionBD");
        }

        // POST: Guardar estudiante (nuevo)
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Lista de estudiantes (vista completa)
        [HttpGet]
        public IActionResult ListaEstudiantes()
        {
            var estudiantes = new List<Estudiante>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ListarEstudiantes", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
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
            return View("~/Views/ListaEstudiante/ListaEstudiante.cshtml", estudiantes);
        }

        // GET: Obtener estudiante por id para mostrar en modal (Detalles y Editar)
        [HttpGet]
        public JsonResult ObtenerEstudiante(int id)
        {
            var estudiante = ObtenerEstudiantePorId(id);
            if (estudiante == null)
                return Json(new { success = false, message = "Estudiante no encontrado" });

            return Json(new { success = true, data = estudiante });
        }

        // AJAX: Actualizar estudiante desde modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ActualizarEstudiante(Estudiante estudiante)
        {
            // Validaciones adicionales para evitar enviar valores inválidos al SP
            if (string.IsNullOrWhiteSpace(estudiante.Nombre) ||
                string.IsNullOrWhiteSpace(estudiante.Apellido) ||
                string.IsNullOrWhiteSpace(estudiante.Correo) ||
                estudiante.Edad <= 0)
            {
                return Json(new { success = false, message = "Complete todos los campos correctamente." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            try
            {
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
                return Json(new { success = true, message = "Estudiante actualizado correctamente" });
            }
            catch (SqlException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Eliminar estudiante (redirecciona a ListaEstudiantes)
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // Método privado para obtener estudiante por Id
        private Estudiante ObtenerEstudiantePorId(int id)
        {
            Estudiante estudiante = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ObtenerEstudiantePorId", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();

                using (var dr = cmd.ExecuteReader())
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
