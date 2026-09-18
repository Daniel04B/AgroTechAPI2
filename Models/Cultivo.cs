using System.ComponentModel.DataAnnotations;

namespace AgroTech.Models
{
    public class Cultivo
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "El nombre del cultivo es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;


        [Required(ErrorMessage = "El tipo de cultivo es obligatorio.")]
        [StringLength(100)]
        public string Tipo { get; set; } = string.Empty;


        [Required(ErrorMessage = "La fecha de siembra es obligatoria.")]
        public DateTime FechaSiembra { get; set; }


        [Required]
        public string Etapa { get; set; } = "Inicial";


        [Required]
        public string Estado { get; set; } = "Activo";


        // Relación con Zona
        public int ZonaId { get; set; }


        // Relación con Agricultor
        public int AgricultorId { get; set; }


        // Datos obtenidos de sensores
        public int? HumedadActual { get; set; }


        public decimal? TemperaturaActual { get; set; }

    }
}