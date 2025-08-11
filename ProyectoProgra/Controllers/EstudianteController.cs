using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
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

            return View(estudiantes);
        }

        // GET: Formulario para RegistroEstudiantes
        [HttpGet]
        public IActionResult RegistroEstudiantes()
        {
            return View();
        }

        // POST: RegistroEstudiantes estudiante
        [HttpPost]
        public IActionResult RegistroEstudiantes(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return View(estudiante);

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
            return RedirectToAction("Lista");
        }

        // GET: Ver detalles de un estudiante
        public IActionResult Detalles(int id)
        {
            var estudiante = ObtenerEstudiantePorId(id);
            if (estudiante == null)
                return NotFound();

            return View(estudiante);
        }

        // GET: Editar estudiante
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var estudiante = ObtenerEstudiantePorId(id);
            if (estudiante == null)
                return NotFound();

            return View(estudiante);
        }

        // POST: Guardar cambios en estudiante
        [HttpPost]
        public IActionResult Editar(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return View(estudiante);

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
            return RedirectToAction("Lista");
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
            return RedirectToAction("Lista");
        }

        // Método privado para obtener estudiante por ID
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
