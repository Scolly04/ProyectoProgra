using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
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

        // GET: Lista de estudiantes
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

        // GET: Datos para ver detalles
        [HttpGet]
        public JsonResult Detalles(int id)
        {
            var estudiante = ObtenerEstudiantePorId(id);
            return Json(estudiante);
        }

        // GET: Datos para editar
        [HttpGet]
        public JsonResult ObtenerParaEditar(int id)
        {
            var estudiante = ObtenerEstudiantePorId(id);
            return Json(estudiante);
        }

        // POST: Editar estudiante
        [HttpPost]
        public IActionResult Editar(Estudiante estudiante)
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
