using System.ComponentModel.DataAnnotations;

namespace AgroTechAPI.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato de correo no es válido (ejemplo@dominio.com).")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña de acceso es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe contener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }

    public class UserSession
    {
        public string Correo { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty; // "Admin" o "Usuario"
        public string Token { get; set; } = string.Empty;
    }
}