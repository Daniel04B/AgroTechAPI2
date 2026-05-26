using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroTechAPI.Models
{
    [Table("Lecturas")]
    public class LecturaSensor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Hora { get; set; } = string.Empty;

        [Required]
        public int Humedad { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Temperatura { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? NivelTanque { get; set; }

        public int? ZonaId { get; set; }
    }
}