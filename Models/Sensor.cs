using System.ComponentModel.DataAnnotations;

namespace AgroTechAPI.Models
{
    public class Sensor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona un tipo")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ubicación es requerida")]
        public string Ubicacion { get; set; } = string.Empty;

        [Range(0.0, 100.0, ErrorMessage = "Debe ser entre 0 y 100")]
        public double ValorCalibracion { get; set; }
    }
}