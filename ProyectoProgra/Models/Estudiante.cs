using System.ComponentModel.DataAnnotations;

namespace TuProyecto.Models
{
    public class Estudiante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La edad es obligatoria")]
        [Range(1, 120, ErrorMessage = "La edad debe ser un valor entre 1 y 120")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; } = string.Empty;
    }
}
