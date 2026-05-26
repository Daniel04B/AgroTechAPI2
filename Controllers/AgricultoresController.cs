using AgroTechAPI.Data;
using AgroTechAPI.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgricultoresController : ControllerBase
    {
        private readonly AgroTechContext _context;

        public AgricultoresController(AgroTechContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agricultor>>> GetAgricultores()
        {
            return await _context.Agricultor.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Agricultor>> GetAgricultor(int id)
        {
            var agricultor = await _context.Agricultor.FindAsync(id);
            if (agricultor == null) return NotFound();
            return agricultor;
        }

        // 1. ENDPOINT DE REGISTRO CORREGIDO
        [HttpPost("registro")]
        public async Task<IActionResult> RegistrarAgricultor([FromBody] Agricultor agricultor)
        {
            // Validar que el objeto no venga nulo
            if (agricultor == null)
            {
                return BadRequest("Los datos del formulario están vacíos o mal formateados.");
            }

            // Limpiar espacios en blanco
            agricultor.Usuario = agricultor.Usuario?.Trim() ?? string.Empty;
            agricultor.Nombre = agricultor.Nombre?.Trim() ?? string.Empty;
            agricultor.Contrasena = agricultor.Contrasena?.Trim() ?? string.Empty;

            // Validar campos obligatorios manualmente por seguridad
            if (string.IsNullOrEmpty(agricultor.Usuario) || string.IsNullOrEmpty(agricultor.Contrasena))
            {
                return BadRequest("El nombre de usuario y la contraseña son obligatorios.");
            }

            // Verificar si el usuario ya existe en la Base de Datos
            var existe = await _context.Agricultor.AnyAsync(a => a.Usuario.ToLower() == agricultor.Usuario.ToLower());
            if (existe)
            {
                return BadRequest("El nombre de usuario ya se encuentra registrado.");
            }

            try
            {
                // Encriptar contraseña de forma segura con BCrypt
                agricultor.Contrasena = BCrypt.Net.BCrypt.HashPassword(agricultor.Contrasena);

                // Forzar rol por defecto si viene vacío
                if (string.IsNullOrEmpty(agricultor.Rol)) agricultor.Rol = "Usuario";

                _context.Agricultor.Add(agricultor);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario registrado con éxito." });
            }
            catch (Exception ex)
            {
                // Esto te dirá en consola si SQL Server rechazó la inserción (ej: por un constraint o llave)
                Console.WriteLine($"Error interno en BD: {ex.Message}");
                return StatusCode(500, $"Error interno al guardar en la base de datos: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // 2. ENDPOINT DE LOGIN CORREGIDO
        [HttpPost("login")]
        public async Task<IActionResult> LoginAgricultor([FromBody] LoginRequest login)
        {
            if (login == null || string.IsNullOrEmpty(login.Usuario) || string.IsNullOrEmpty(login.Contrasena))
            {
                return Unauthorized("Usuario y contraseña requeridos.");
            }

            string userTrim = login.Usuario.Trim();
            string passTrim = login.Contrasena.Trim();

            // Buscar al agricultor por su usuario
            var agricultor = await _context.Agricultor
                .FirstOrDefaultAsync(a => a.Usuario.ToLower() == userTrim.ToLower());

            // Si no existe el usuario
            if (agricultor == null)
            {
                return Unauthorized("El usuario no existe o la contraseña es incorrecta.");
            }

            // Verificar la contraseña encriptada
            bool contraseñaCorrecta = false;
            try
            {
                contraseñaCorrecta = BCrypt.Net.BCrypt.Verify(passTrim, agricultor.Contrasena);
            }
            catch (Exception)
            {
                // Si la contraseña en la BD no era un Hash de BCrypt (era texto plano viejo), dará excepción
                return Unauthorized("Contraseña desactualizada. Por favor, registre un usuario nuevo.");
            }

            if (!contraseñaCorrecta)
            {
                return Unauthorized("El usuario no existe o la contraseña es incorrecta.");
            }

            // Retornar el objeto para iniciar sesión, limpiando el hash por seguridad
            agricultor.Contrasena = string.Empty;
            return Ok(agricultor);
        }
        // Clase auxiliar necesaria para recibir el Login sin alterar el modelo principal
        public class LoginRequest
        {
            public string Usuario { get; set; } = string.Empty;
            public string Contrasena { get; set; } = string.Empty;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgricultor(int id, Agricultor agricultor)
        {
            if (id != agricultor.Id) return BadRequest();

            _context.Entry(agricultor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Agricultor.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgricultor(int id)
        {
            var agricultor = await _context.Agricultor.FindAsync(id);
            if (agricultor == null) return NotFound();

            _context.Remove(agricultor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}