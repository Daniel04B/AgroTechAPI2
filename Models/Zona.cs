using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroTechAPI.Models
{
    [Table("Zonas")]
    public class Zona
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50)]
        public string? SensorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = string.Empty;

        public int? Humedad { get; set; }

        public int? AgricultorId { get; set; }
    }
}