using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroTechAPI.Models
{
    [Table("Sensor")]
    public class Sensor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Ubicacion { get; set; } = string.Empty;

        public decimal? ValorCalibracion { get; set; }

        public decimal? ValorActual { get; set; }

        public DateTime? FechaLectura { get; set; }

        public int? AgricultorId { get; set; }
    }
}