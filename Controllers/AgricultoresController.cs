using AgroTechAPI.Data;
using AgroTechAPI.Models;
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

        // =========================================
        // GET: api/agricultores
        // =========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agricultor>>> GetAgricultores()
        {
            try
            {
                return await _context.Agricultor.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno: {ex.Message}");
            }
        }

        // =========================================
        // GET: api/agricultores/5
        // =========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<Agricultor>> GetAgricultor(int id)
        {
            try
            {
                var agricultor = await _context.Agricultor.FindAsync(id);

                if (agricultor == null)
                    return NotFound("Agricultor no encontrado.");

                agricultor.Contrasena = "";

                return agricultor;
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno: {ex.Message}");
            }
        }

        // =========================================
        // REGISTRO
        // =========================================
        [HttpPost("registro")]
        public async Task<IActionResult> RegistrarAgricultor(
            [FromBody] Agricultor agricultor)
        {
            try
            {
                if (agricultor == null)
                    return BadRequest("Datos inválidos.");

                agricultor.Nombre =
                    agricultor.Nombre?.Trim() ?? "";

                agricultor.Usuario =
                    agricultor.Usuario?.Trim() ?? "";

                agricultor.Contrasena =
                    agricultor.Contrasena?.Trim() ?? "";

                // VALIDACIONES
                if (string.IsNullOrWhiteSpace(agricultor.Nombre))
                    return BadRequest("El nombre es obligatorio.");

                if (string.IsNullOrWhiteSpace(agricultor.Usuario))
                    return BadRequest("El usuario es obligatorio.");

                if (string.IsNullOrWhiteSpace(agricultor.Contrasena))
                    return BadRequest("La contraseña es obligatoria.");

                if (agricultor.Contrasena.Length < 5)
                    return BadRequest(
                        "La contraseña debe tener al menos 5 caracteres.");

                // VALIDAR DUPLICADOS
                bool existe = await _context.Agricultor
                    .AnyAsync(a =>
                        a.Usuario.ToLower() ==
                        agricultor.Usuario.ToLower());

                if (existe)
                    return BadRequest(
                        "El usuario ya existe.");

                // ENCRIPTAR CONTRASEÑA
                agricultor.Contrasena =
                    BCrypt.Net.BCrypt.HashPassword(
                        agricultor.Contrasena);

                // ROL POR DEFECTO
                // Validar roles permitidos

                if (string.IsNullOrEmpty(agricultor.Rol))
                {
                    agricultor.Rol = "Usuario";
                }

                if (agricultor.Rol != "Admin" &&
                    agricultor.Rol != "Usuario")
                {
                    return BadRequest("Rol inválido.");
                }

                _context.Agricultor.Add(agricultor);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Usuario registrado correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno: {ex.Message}");
            }
        }

        // =========================================
        // LOGIN
        // =========================================
        [HttpPost("login")]
        public async Task<IActionResult> LoginAgricultor(
            [FromBody] LoginRequest login)
        {
            try
            {
                if (login == null)
                    return Unauthorized(
                        "Datos inválidos.");

                if (string.IsNullOrWhiteSpace(login.Usuario))
                    return Unauthorized(
                        "Usuario requerido.");

                if (string.IsNullOrWhiteSpace(login.Contrasena))
                    return Unauthorized(
                        "Contraseña requerida.");

                string usuario =
                    login.Usuario.Trim();

                string contraseña =
                    login.Contrasena.Trim();

                var agricultor = await _context.Agricultor
                    .FirstOrDefaultAsync(a =>
                        a.Usuario.ToLower() ==
                        usuario.ToLower());

                if (agricultor == null)
                    return Unauthorized(
                        "Usuario o contraseña incorrectos.");

                bool contraseñaCorrecta =
                    BCrypt.Net.BCrypt.Verify(
                        contraseña,
                        agricultor.Contrasena);

                if (!contraseñaCorrecta)
                    return Unauthorized(
                        "Usuario o contraseña incorrectos.");

                agricultor.Contrasena = "";

                return Ok(agricultor);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno: {ex.Message}");
            }
        }

        // =========================================
        // PUT
        // =========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgricultor(
            int id,
            Agricultor agricultor)
        {
            try
            {
                if (id != agricultor.Id)
                    return BadRequest(
                        "ID incorrecto.");

                var agricultorBD =
                    await _context.Agricultor
                    .FindAsync(id);

                if (agricultorBD == null)
                    return NotFound(
                        "Agricultor no encontrado.");

                agricultorBD.Nombre =
                    agricultor.Nombre;

                agricultorBD.Apellido =
                    agricultor.Apellido;

                agricultorBD.Usuario =
                    agricultor.Usuario;

                agricultorBD.Rol =
                    agricultor.Rol;

                await _context.SaveChangesAsync();

                return Ok(
                    "Agricultor actualizado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno: {ex.Message}");
            }
        }

        // =========================================
        // DELETE
        // =========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgricultor(
            int id)
        {
            try
            {
                var agricultor =
                    await _context.Agricultor
                    .FindAsync(id);

                if (agricultor == null)
                    return NotFound(
                        "Agricultor no encontrado.");

                _context.Agricultor.Remove(agricultor);

                await _context.SaveChangesAsync();

                return Ok(
                    "Agricultor eliminado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno: {ex.Message}");
            }
        }

        // =========================================
        // MODELO LOGIN
        // =========================================
        public class LoginRequest
        {
            public string Usuario { get; set; } = "";
            public string Contrasena { get; set; } = "";
        }
    }
}