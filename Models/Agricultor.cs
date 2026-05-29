using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroTechAPI.Models
{

    [Table("Agricultor")]
    public class Agricultor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Apellido { get; set; }

        [Required]
        [StringLength(150)]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        //[JsonIgnore] // Seguridad: Bloquea la contraseña en respuestas JSON de lectura
        //public string Contrasena { get; set; } = string.Empty;
        // Esto le dice al serializador: "No importa qué pase, lee 'contrasena' del JSON"
        [JsonPropertyName("contrasena")]
        public string Contrasena { get; set; } = string.Empty;

        [StringLength(50)]
        public string Rol { get; set; } = "Usuario";
    }
}